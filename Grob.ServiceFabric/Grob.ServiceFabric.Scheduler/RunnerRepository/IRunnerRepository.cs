using Grob.ServiceFabric.Scheduler.Schedule;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grob.ServiceFabric.Scheduler.RunnerRepository
{
    interface IRunnerRepository
    {
        Task AddRunnerAsync(BaseScheduleRunner runner);
        Task<List<BaseScheduleRunner>> GetRunners();
        Task StopRunner(Guid runnerId);
        Task<BaseScheduleRunner> GetRunner(Guid grobTask);
    }
}
