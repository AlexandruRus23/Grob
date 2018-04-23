using Grob.Entities.Grob;
using Grob.Master.Models;
using NCrontab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grob.ServiceFabric.Scheduler.Schedule
{
    public class TimerSchedulerImpl : IScheduleRunner
    {
        private GrobTask _grobTask;
        private IGrobMasterService _grobMasterService;

        public TimerSchedulerImpl(GrobTask grobTask, IGrobMasterService grobMasterService)
        {
            _grobTask = grobTask;
            _grobMasterService = grobMasterService;
        }

        public void Start()
        {
            while (true)
            {
                var schedule = CrontabSchedule.Parse(_grobTask.ScheduleInfo);
                var nextRun = schedule.GetNextOccurrence(DateTime.Now);

                var sleepTime = nextRun.Subtract(DateTime.Now);
                Thread.Sleep(sleepTime);

                Thread thread = new Thread(RunAsync);
                thread.Start();
            }
        }

        public void RunAsync()
        {
            _grobMasterService.RunTask(_grobTask);
        }        
    }
}
