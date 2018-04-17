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

namespace Grob.ServiceFabric.Agent
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class Agent : StatelessService, IGrobAgentService
    {
        private IDockerManager _dockerManager;
        private IGrobMasterService _grobMasterService;

        public Agent(StatelessServiceContext context)
            : base(context)
        {
            _dockerManager = new StubDockerManager();
            //_dockerManager = new DockerManager();

            _grobMasterService = ServiceProxy.Create<IGrobMasterService>(new Uri("fabric:/Grob.ServiceFabric/Grob.ServiceFabric.Master"), new ServicePartitionKey(1));
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

            EndpointResourceDescription internalEndpoint = Context.CodePackageActivationContext.GetEndpoint("ProcessingServiceEndpoint");

            string uriPrefix = String.Format(
                "{0}://+:{1}/{2}/{3}-{4}/",
                internalEndpoint.Protocol,
                internalEndpoint.Port,
                Context.PartitionId,
                Context.ReplicaOrInstanceId,
                Guid.NewGuid());

            string nodeIP = FabricRuntime.GetNodeContext().IPAddressOrFQDN;

            string uriPublished = uriPrefix.Replace("+", nodeIP);
            return new HttpCommunicationListener(uriPrefix, uriPublished, this.ProcessInternalRequest);
        }

        private async Task ProcessInternalRequest(HttpListenerContext context, CancellationToken cancelRequest)
        {
            string output = "asd";

            using (HttpListenerResponse response = context.Response)
            {
                if (output != null)
                {
                    byte[] outBytes = Encoding.UTF8.GetBytes(output);
                    response.OutputStream.Write(outBytes, 0, outBytes.Length);
                }
            }
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            await _grobMasterService.RegisterAgentAsync(Partition.PartitionInfo.Id.ToString());
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

        private void RegisterToScheduler()
        {
            var agent = new GrobAgent(Environment.MachineName, new Uri(GetLocalIPAddress()));

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:8609/api/agent");

            client.SendAsync(request);
        }

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
