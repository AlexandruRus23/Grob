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

namespace Grob.ServiceFabric.Master
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class Master : StatefulService, IGrobMasterService
    {
        private IGrobAgentService _grobAgent;
        private IContainerRepository _containerRepository;

        public Master(StatefulServiceContext context)
            : base(context)
        {
            _grobAgent = ServiceProxy.Create<IGrobAgentService>(new Uri("fabric:/Grob.ServiceFabric/Grob.ServiceFabric.Agent"));
            _containerRepository = new ServiceFabricContainerRepository(this.StateManager);
        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return this.CreateServiceRemotingReplicaListeners();
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            List<Container> containers = await _grobAgent.GetContainersAsync();
            containers.ForEach(c => _containerRepository.AddContainerAsync(c));
        }

        public async Task RunTask(GrobTask task)
        {
            var containers = await _containerRepository.GetAllContainersAsync();

            var container = containers.Where(c => c.Image == task.Name)?.FirstOrDefault();

            if(container != null)
            {
                await _grobAgent.RunContainerAsync(container);
            }
        }

        public async Task<List<Container>> GetContainersAsync()
        {
            return await _containerRepository.GetAllContainersAsync();
        }
    }
}
