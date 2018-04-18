using Grob.Entities.Grob;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grob.ServiceFabric.Scheduler.JobRepository
{
    interface ITaskRepository
    {
        Task AddTask(GrobTask job);
        Task<IEnumerable<GrobTask>> GetTasks();
    }
}
