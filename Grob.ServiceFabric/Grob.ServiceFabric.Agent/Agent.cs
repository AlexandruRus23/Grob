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

namespace Grob.ServiceFabric.Agent
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class Agent : StatelessService, IGrobAgentService
    {
        private IDockerManager _dockerManager;

        public Agent(StatelessServiceContext context)
            : base(context)
        {
            _dockerManager = new StubDockerManager();
            //_dockerManager = new DockerManager();
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {

            return this.CreateServiceRemotingInstanceListeners();
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.            

            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var list = _dockerManager.ListContainers().Result;

                if (list != null)
                {
                    foreach (var container in list)
                    {
                        var command = new GrobAgentCommand(GrobAgentCommandTypeEnum.RunImage, container.ID);

                        var executor = new CommandExecutor(command);
                        executor.Run();
                    }
                }                

                await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);
            }
        }        

        public async Task RunJob(Job job)
        {
            await _dockerManager.StartContainer(job.Name);
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
