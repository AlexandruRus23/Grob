using Grob.ServiceFabric.Scheduler.Schedule;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grob.ServiceFabric.Scheduler.RunnerRepository
{
    interface IRunnerRepository
    {
        Task AddRunnerAsync(IScheduleRunner runner);
        Task<List<IScheduleRunner>> GetRunners();
        Task StopRunner(Guid runnerId);
        Task<IScheduleRunner> GetRunner(Guid grobTask);
    }
}
