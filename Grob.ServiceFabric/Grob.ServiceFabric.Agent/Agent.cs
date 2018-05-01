using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Grob.Docker;
using Grob.Agent.Models;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Grob.Entities.Grob;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Docker.DotNet.Models;
using Grob.Entities.Docker;
using Grob.Master.Models;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Client;
using System.Fabric.Description;
using System.Text;
using System.Diagnostics;

namespace Grob.ServiceFabric.Agent
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class Agent : StatelessService
    {
        private IDockerManager _dockerManager;
        private IGrobMasterService _grobMasterService;
        private OwinCommunicationListener communicationListener;

        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;

        public Agent(StatelessServiceContext context)
            : base(context)
        {
            //_dockerManager = new StubDockerManager();
            _dockerManager = new DockerManager();

            _grobMasterService = ServiceProxy.Create<IGrobMasterService>(new Uri("fabric:/Grob.ServiceFabric/Grob.ServiceFabric.Master"), new ServicePartitionKey(1));
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            // Service remoting
            //return this.CreateServiceRemotingInstanceListeners();

            // HTTP listener
            //return new[] { new ServiceInstanceListener(context => new HttpCommunicationListener(context)) };
            communicationListener = new OwinCommunicationListener("agent", new Startup(), Context);

            return new[]
            {
                new ServiceInstanceListener(context => communicationListener)
            };
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            var grobAgent = new GrobAgent(Environment.MachineName, communicationListener.GrobAgentAddress, Context.InstanceId, Context.NodeContext.NodeName);
            await _grobMasterService.RegisterAgentAsync(grobAgent);

            while (true)
            {
                var agentInformation = new AgentInformation()
                {
                    CpuUsage = cpuCounter.NextValue().ToString(),
                    AvailableMemory = ramCounter.NextValue().ToString()
                };

                AgentResourceUtilization.AddAgentInformation(agentInformation);
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }

        public async Task RunContainerAsync(Container container)
        {
            await _dockerManager.StartContainerAsync(container);
        }

        public async Task<List<Container>> GetContainersAsync()
        {
            return await _dockerManager.ListContainers();
        }

        #region private

        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        #endregion
    }
}
