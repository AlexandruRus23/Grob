using Grob.Entities.Grob;
using Grob.Master.Models;
using NCrontab;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grob.ServiceFabric.Scheduler.Schedule
{
    public class TimerSchedulerImpl : BaseScheduleRunner
    {
        public TimerSchedulerImpl(GrobTask grobTask, IGrobMasterService grobMasterService) : base(grobTask, grobMasterService)
        {
            RunnerThread = new Thread(Start);
            RunnerThread.Start();
        }

        public override void Start()
        {
            while (true)
            {
                var schedule = CrontabSchedule.Parse(GrobTask.ScheduleInfo);
                var nextRun = schedule.GetNextOccurrence(DateTime.Now);
                GrobTask.NextRunTime = nextRun.ToString();

                var sleepTime = nextRun.Subtract(DateTime.Now);
                Thread.Sleep(sleepTime);
                Thread thread = new Thread(RunAsync);
                thread.Start();      
            }
        }

        public override void RunAsync()
        {
            GrobTask.LastRunTime = DateTime.Now.ToString();
            GrobMasterService.RunTask(GrobTask);
        }

        public override void Stop()
        {
            GrobMasterService.StopTask(GrobTask);
            RunnerThread?.Abort();
        }
    }
}
