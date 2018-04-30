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
    public abstract class BaseScheduleRunner
    {
        public Guid Id { get; set; }

        public Thread RunnerThread { get; set; }
        protected GrobTask GrobTask { get; set; }
        protected IGrobMasterService GrobMasterService { get; set; }

        public BaseScheduleRunner(GrobTask grobTask, IGrobMasterService grobMasterService)
        {
            GrobTask = grobTask;
            GrobMasterService = grobMasterService;
            Id = grobTask.Id;
        }

        public abstract void Start();
        public abstract void Run();
        public abstract Task RunAsync();
        public abstract void Stop();
    }
}
