using Grob.Entities.Grob;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grob.ServiceFabric.Scheduler.TaskRepository
{
    interface ITaskRepository
    {
        Task AddTask(GrobTask job);
        Task<List<GrobTask>> GetTasks();
        Task DeleteTaskAsync(Guid taskId);
    }
}
