using Grob.Entities.Grob;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grob.ServiceFabric.Scheduler.JobRepository
{
    interface IJobRepository
    {
        Task AddJob(GrobJob job);
        Task<IEnumerable<GrobJob>> GetJobs();
    }
}
