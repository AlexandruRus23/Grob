using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grob.ServiceFabric.Scheduler.Schedule
{
    public interface IScheduleRunner
    {
        void Start();
        Task RunAsync();
    }
}
