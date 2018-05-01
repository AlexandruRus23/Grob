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
                Thread thread = new Thread(Run);
                thread.Start();      
            }
        }

        public override void Run()
        {
            GrobTask.LastRunTime = DateTime.Now.ToString();
            var logs = GrobMasterService.RunTaskAsync(GrobTask).Result;
            GrobTask.Logs.Add(logs);
        }

        public override void Stop()
        {
            GrobMasterService.StopTask(GrobTask);
            RunnerThread?.Abort();
        }

        public override async Task RunAsync()
        {
            GrobTask.LastRunTime = DateTime.Now.ToString();
            await GrobMasterService.RunTaskAsync(GrobTask);
        }
    }
}
