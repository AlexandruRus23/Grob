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
    public class TimerSchedulerImpl : IScheduleRunner
    {
        public Guid Id { get; set; }
        public DateTime NextRun { get; set; }
        public DateTime LastRun { get; set; }

        private bool _isRunning; 

        private GrobTask _grobTask;
        private IGrobMasterService _grobMasterService;        

        public TimerSchedulerImpl(GrobTask grobTask, IGrobMasterService grobMasterService)
        {
            _grobTask = grobTask;
            _grobMasterService = grobMasterService;
            _isRunning = true;
            Id = grobTask.Id;

            var thread = new Thread(Start);
            thread.Start();
        }

        public void Start()
        {
            while (_isRunning)
            {
                var schedule = CrontabSchedule.Parse(_grobTask.ScheduleInfo);
                var nextRun = schedule.GetNextOccurrence(DateTime.Now);
                NextRun = nextRun;

                var sleepTime = nextRun.Subtract(DateTime.Now);
                Thread.Sleep(sleepTime);

                if (!_isRunning)
                {
                    break;
                }

                Thread thread = new Thread(RunAsync);
                thread.Start();      
            }
        }

        public void RunAsync()
        {
            LastRun = DateTime.Now;
            _grobMasterService.RunTask(_grobTask);
        }

        public void Stop()
        {
            _isRunning = false;
        }
    }
}
