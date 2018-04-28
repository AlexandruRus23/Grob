using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grob.Entities.Grob;
using Grob.ServiceFabric.Scheduler.Schedule;

namespace Grob.ServiceFabric.Scheduler.RunnerRepository
{
    public class SimpleRunnerRepository : IRunnerRepository
    {
        private List<BaseScheduleRunner> scheduleRunners;

        public SimpleRunnerRepository()
        {
            scheduleRunners = new List<BaseScheduleRunner>();
        }

        public async Task AddRunnerAsync(BaseScheduleRunner runner)
        {
            scheduleRunners.Add(runner);
        }

        public async Task<BaseScheduleRunner> GetRunner(Guid runnerId)
        {
            return scheduleRunners.Where(r => r.Id == runnerId).FirstOrDefault();
        }

        public async Task<List<BaseScheduleRunner>> GetRunners()
        {
            return scheduleRunners;
        }

        public async Task StopRunner(Guid runnerId)
        {
            var runner = scheduleRunners.Where(r => r.Id == runnerId).FirstOrDefault();
            runner?.Stop();
        }
    }
}
