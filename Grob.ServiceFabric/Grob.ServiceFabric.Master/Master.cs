using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Grob.Master.Models;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Grob.Agent.Models;
using Grob.Entities.Grob;
using Grob.ServiceFabric.Master.ContainerRepository;
using Grob.Entities.Docker;
using Microsoft.ServiceFabric.Services.Client;
using Grob.ServiceFabric.Master.AgentRepository;
using System.IO;

namespace Grob.ServiceFabric.Master
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class Master : StatefulService, IGrobMasterService
    {
        private IGrobAgentRepository _grobAgentRepository;
        private IContainerRepository _containerRepository;
        private FabricClient _fabricClient;

        public Master(StatefulServiceContext context)
            : base(context)
        {        
            _containerRepository = new ServiceFabricContainerRepository(this.StateManager);
            _grobAgentRepository = new ServiceFabricAgentRepository(this.StateManager);
            _fabricClient = new FabricClient();
        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            // Service remoting
            return this.CreateServiceRemotingReplicaListeners();
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            //foreach(var agent in _grobAgentServices)
            //{
            //    List<Container> containers = agent.GetContainers();
            //    containers.ForEach(c => _containerRepository.AddContainerAsync(c));
            //}           
        }

        public async Task RunTask(GrobTask task)
        {
            var containers = await GetContainersAsync();

            var container = containers.Where(c => c.Name == task.Name.ToLower().Replace(" ", string.Empty))?.FirstOrDefault();

            if (container != null)
            {
                var agents = await GetLeastUsedAgentAsync();
                agents.RunContainer(container);
            }
        }

        public async Task<List<Container>> GetContainersAsync()
        {
            var result = new List<Container>();

            foreach (var agent in await _grobAgentRepository.GetGrobAgentsAsync())
            {
                result.AddRange(agent.GetContainers());
            }

            return result;
        }

        public async Task RegisterAgentAsync(GrobAgent grobAgent)
        {
            await _grobAgentRepository.AddAgent(grobAgent);
        }

        public async Task<List<GrobAgent>> GetGrobAgentsAsync()
        {
            var agents = await _grobAgentRepository.GetGrobAgentsAsync();
            
            foreach(var agent in agents)
            {
                var information = agent.GetAgentInformation();
                agent.CpuUsage = information.CpuUsage;
                agent.AvailableMemory = information.AvailableMemory;
            }

            return agents;
        }

        private async Task<GrobAgent> GetLeastUsedAgentAsync()
        {
            var allAgents = await _grobAgentRepository.GetGrobAgentsAsync();
            float minUsage = 100;
            var selectedAgent = new GrobAgent();

            foreach(var agent in allAgents)
            {
                var information = agent.GetAgentInformation();
                float.TryParse(information.CpuUsage, out float value);

                if(minUsage - value > 0)
                {
                    selectedAgent = agent;
                }
            }

            return selectedAgent;
        }

        public async Task<List<Application>> GetApplicationsAsync()
        {
            var agent = await GetLeastUsedAgentAsync();
            return agent.GetApplications();
        }

        public async Task CreateContainerForTaskAsync(GrobTask grobTask)
        {
            var agents = await _grobAgentRepository.GetGrobAgentsAsync();
            agents.ForEach(a => a.CreateContainers(grobTask));
        }

        public async Task DeleteContainerForTaskAsync(GrobTask grobTask)
        {
            _grobAgentRepository.GetGrobAgentsAsync().Result.ForEach(a => a.DeleteContainers(grobTask));
        }
    }
}
