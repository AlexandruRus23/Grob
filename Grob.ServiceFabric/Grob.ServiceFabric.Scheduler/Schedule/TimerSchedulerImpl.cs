using Grob.Entities.Grob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grob.ServiceFabric.Scheduler.Schedule
{
    public class TimerSchedulerImpl : IScheduleRunner
    {
        private GrobTask _grobTask;

        public TimerSchedulerImpl(GrobTask grobTask)
        {
            _grobTask = grobTask;
        }

        public void Start()
        {
            
        }

        public Task RunAsync()
        {
            throw new NotImplementedException();
        }        
    }
}
