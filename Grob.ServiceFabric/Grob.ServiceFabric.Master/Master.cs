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

namespace Grob.ServiceFabric.Master
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class Master : StatefulService, IGrobMasterService
    {
        private IGrobAgentService _grobAgent;

        public Master(StatefulServiceContext context)
            : base(context)
        {
            _grobAgent = ServiceProxy.Create<IGrobAgentService>(new Uri("fabric:/Grob.ServiceFabric/Grob.ServiceFabric.Agent"));
        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return this.CreateServiceReplicaListeners();
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
        }

        public Task RunJob(Task task)
        {
            throw new NotImplementedException();
        }        
    }
}
