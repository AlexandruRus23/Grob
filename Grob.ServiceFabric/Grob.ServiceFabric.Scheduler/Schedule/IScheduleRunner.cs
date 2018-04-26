using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grob.ServiceFabric.Scheduler.Schedule
{
    public interface IScheduleRunner
    {
        Guid Id { get; set; }

        void Start();
        void RunAsync();
        void Stop();
    }
}
