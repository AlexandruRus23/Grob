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
        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return this.CreateServiceRemotingReplicaListeners();
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            //while (true)
            //{
            //    var containers = await _grobAgent.GetContainersAsync();
            //    containers.ToList().ForEach(c => _containerRepository.AddContainerAsync(c));
            //    await Task.Delay(TimeSpan.FromSeconds(60), cancellationToken);
            //}
        }

        public async Task RunTask(GrobTask task)
        {
            var containers = await _containerRepository.GetAllContainersAsync();

            var container = containers.Where(c => c.Name == task.Name)?.FirstOrDefault();

            if(container != null)
            {
                await _grobAgent.RunContainerAsync(container);
            }
        }        
    }
}
