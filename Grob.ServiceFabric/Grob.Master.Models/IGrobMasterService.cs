using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grob.Master.Models
{
    public interface IGrobMasterService
    {
        Task RunJob(Task task);
    }
}
