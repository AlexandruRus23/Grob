using Grob.Entities.Grob;
using Grob.Master.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grob.ServiceFabric.Scheduler.Schedule
{
    public class WebHostScheduler : BaseScheduleRunner
    {
        public WebHostScheduler(GrobTask grobTask, IGrobMasterService grobMasterService) : base(grobTask, grobMasterService)
        {
        }        

        public override void Start()
        {
            var thread = new Thread(Run);
            thread.Start();
        }

        public override void Run()
        {
            GrobTask.Status = GrobTaskStatusEnum.Running;
            GrobMasterService.RunTaskAsync(GrobTask);            
        }

        public override void Stop()
        {
            GrobTask.Status = GrobTaskStatusEnum.Stopped;
            GrobMasterService.StopTask(GrobTask);
            RunnerThread?.Abort();
        }

        public override async Task RunAsync()
        {
            GrobTask.Status = GrobTaskStatusEnum.Running;
            await GrobMasterService.RunTaskAsync(GrobTask);
        }
    }
}
