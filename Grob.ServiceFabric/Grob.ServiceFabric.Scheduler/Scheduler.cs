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
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Client;
using Grob.Master.Models;
using Grob.Scheduler.Models;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Grob.ServiceFabric.Scheduler.TaskRepository;
using Grob.ServiceFabric.Scheduler.Schedule;
using Grob.ServiceFabric.Scheduler.RunnerRepository;

namespace Grob.ServiceFabric.Scheduler
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class Scheduler : StatefulService, IGrobSchedulerService
    {
        private ITaskRepository _taskRepository;
        private IRunnerRepository _runnerRepository;
        private IGrobMasterService _grobMaster;

        public Scheduler(StatefulServiceContext context)
            : base(context)
        {
            _taskRepository = new ServiceFabricTaskRepository(this.StateManager);

            //_runnerRepository = new ServiceFabricRunnerRepository(this.StateManager);
            _runnerRepository = new SimpleRunnerRepository();

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
            return this.CreateServiceRemotingReplicaListeners();
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // register old tasks
            foreach(var task in await _taskRepository.GetTasks())
            {
                await CreateTaskAsync(task);
            }
        }

        public async Task<List<GrobTask>> GetTasksAsync()
        {
            return await _taskRepository.GetTasks();
        }

        public async Task<GrobTask> GetTaskAsync(string taskName)
        {
            var allTasks = await _taskRepository.GetTasks();
            return allTasks.Where(t => t.Name == taskName).FirstOrDefault();
        }

        public async Task CreateTaskAsync(GrobTask task)
        {
            // CHANGE THIS TO PUBLIC IP OF CLUSTER
            task.PublicUrl = $"http://localhost:8080/api/GrobTaskRunner/{task.Name}";
            await _taskRepository.AddTask(task);

            if(task.ScheduleType != ScheduleTypesEnum.WebTrigger)
            {
                await StartTaskAsync(task);
            }            
        }

        public async Task DeleteTaskAsync(Guid taskId)
        {
            var task = GetTasksAsync().Result.Where(t => t.Id == taskId).FirstOrDefault();
            await _taskRepository.DeleteTaskAsync(taskId);
            await _runnerRepository.StopRunner(task.Id);
            await _grobMaster.DeleteContainerForTaskAsync(task);
        }

        public async Task<string> StartTaskAsync(GrobTask grobTaskToRun)
        {
            var registeredTask = await _taskRepository.GetRegisteredTask(grobTaskToRun.Name);

            if(registeredTask.ScheduleType == ScheduleTypesEnum.WebTrigger)
            {
                registeredTask.LastRunTime = DateTime.Now.ToString();
            }            

            if(registeredTask.Status == GrobTaskStatusEnum.Stopped)
            {
                registeredTask.Status = GrobTaskStatusEnum.Running;
                var runner = SchedulerFactory.GetRunner(registeredTask, _grobMaster);

                // if task is webtrigger, then wait for the task to run
                if (registeredTask.ScheduleType == ScheduleTypesEnum.WebTrigger)
                {
                    await runner.RunAsync();
                    //Thread.Sleep(500);
                }

                runner.RunnerThread = new Thread(runner.Start);
                runner.RunnerThread.Start();

                await _runnerRepository.AddRunnerAsync(runner);
            }

            return registeredTask.PrivateUrl;
        }
    }
}
