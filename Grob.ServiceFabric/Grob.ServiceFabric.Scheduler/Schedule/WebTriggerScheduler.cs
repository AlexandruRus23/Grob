using Grob.Entities.Grob;
using Grob.Master.Models;
using Grob.ServiceFabric.Scheduler.TaskRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grob.ServiceFabric.Scheduler.Schedule
{
    public class WebTriggerScheduler : BaseScheduleRunner
    {
        private ITaskRepository taskRepository;

        public WebTriggerScheduler(GrobTask grobTask, IGrobMasterService grobMasterService) : base(grobTask, grobMasterService)
        {
        }

        public override void Start()
        {
            GrobTask.LastRunTime = DateTime.Now.ToString();
            var lastRunTime = DateTime.Parse(GrobTask.LastRunTime);

            while (DateTime.Now.Subtract(lastRunTime).Minutes < 2)
            {
                Thread.Sleep(lastRunTime.AddMinutes(2).Subtract(DateTime.Now));
            }

            GrobTask.Status = GrobTaskStatusEnum.Stopped;
            GrobMasterService.StopTask(GrobTask);
        }

        public override void Run()
        {
            GrobMasterService.RunTaskAsync(GrobTask);
        }

        public override async Task RunAsync()
        {
            var uri = await GrobMasterService.RunTaskAsync(GrobTask);
            GrobTask.PrivateUrl = uri;
        }

        public override void Stop()
        {
            GrobTask.Status = GrobTaskStatusEnum.Stopped;
            GrobMasterService.StopTask(GrobTask);
            RunnerThread?.Abort();
        }        
    }
}
