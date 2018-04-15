using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Grob.Entities.Grob;
using Grob.ServiceFabric.Scheduler.JobRepository;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Client;
using Grob.Master.Models;

namespace Grob.ServiceFabric.Scheduler
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class Scheduler : StatefulService
    {
        private IJobRepository _jobRepository;
        private IGrobMasterService _grobMaster;

        public Scheduler(StatefulServiceContext context)
            : base(context)
        {
            _jobRepository = new JobRepository.JobRepository(this.StateManager);
            _grobMaster = ServiceProxy.Create<IGrobMasterService>(new Uri("fabric:/Grob.ServiceFabric/Grob.ServiceFabric.Master"), new ServicePartitionKey(1));
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new ServiceReplicaListener[0];
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                //var Job = new GrobTask("/clever_haibt", "test", "tag");
                //var Job2 = new GrobJob("job2", "test", "tag");
                //var Job3 = new GrobJob("job3", "test", "tag");

                //await _jobRepository.AddJob(Job);
                //await _jobRepository.AddJob(Job);
                //await _jobRepository.AddJob(Job);

                //var task = new GrobTask(Job);

                //await _grobMaster.RunJob(task);

                await Task.Delay(TimeSpan.FromSeconds(20), cancellationToken);
            }            
        }
    }
}
