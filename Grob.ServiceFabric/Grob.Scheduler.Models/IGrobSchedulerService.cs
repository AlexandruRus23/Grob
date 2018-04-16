using Grob.Entities.Grob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grob.Scheduler.Models
{
    public interface IGrobSchedulerService
    {
        Task<List<GrobTask>> GetTasks();
    }
}
