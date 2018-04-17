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

namespace Grob.ServiceFabric.Master
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class Master : StatefulService, IGrobMasterService
    {
        private List<IGrobAgentService> _grobAgentServices;
        private IContainerRepository _containerRepository;

        public Master(StatefulServiceContext context)
            : base(context)
        {
            _grobAgentServices = new List<IGrobAgentService>();
            //_grobAgentRepository = ServiceProxy.Create<IGrobAgentService>(new Uri("fabric:/Grob.ServiceFabric/Grob.ServiceFabric.Agent"), );            
            _containerRepository = new ServiceFabricContainerRepository(this.StateManager);
        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            // Service remoting
            return this.CreateServiceRemotingReplicaListeners();            
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            foreach(var agent in _grobAgentServices)
            {
                List<Container> containers = await agent.GetContainersAsync();
                containers.ForEach(c => _containerRepository.AddContainerAsync(c));
            }           
        }

        public async Task RunTask(GrobTask task)
        {
            var containers = await _containerRepository.GetAllContainersAsync();

            var container = containers.Where(c => c.Image == task.Name)?.FirstOrDefault();

            if(container != null)
            {
                await _grobAgentServices.FirstOrDefault()?.RunContainerAsync(container);
            }
        }

        public async Task<List<Container>> GetContainersAsync()
        {
            var result = new List<Container>();

            foreach(var agent in _grobAgentServices)
            {
                result.AddRange(await agent.GetContainersAsync());
            }

            return result;
        }

        public async Task RegisterAgentAsync(string partitionKey)
        {
            _grobAgentServices.Add(ServiceProxy.Create<IGrobAgentService>(new Uri("fabric:/Grob.ServiceFabric/Grob.ServiceFabric.Agent")));
        }
    }
}
