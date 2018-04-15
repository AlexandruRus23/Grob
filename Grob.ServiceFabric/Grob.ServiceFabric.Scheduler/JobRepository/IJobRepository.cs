using Grob.Entities.Grob;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grob.ServiceFabric.Scheduler.JobRepository
{
    interface IJobRepository
    {
        Task AddJob(GrobTask job);
        Task<IEnumerable<GrobTask>> GetJobs();
    }
}
