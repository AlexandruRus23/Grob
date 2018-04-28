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
            RunnerThread = new Thread(Start);
            RunnerThread.Start();
        }

        public override void RunAsync()
        {
            RunnerThread = new Thread(Start);
            RunnerThread.Start();
        }

        public override void Start()
        {
            GrobTask.Status = GrobTaskStatusEnum.Running;
            GrobMasterService.RunTask(GrobTask);
        }

        public override void Stop()
        {
            GrobTask.Status = GrobTaskStatusEnum.Stopped;
            GrobMasterService.StopTask(GrobTask);
            RunnerThread?.Abort();
        }
    }
}
