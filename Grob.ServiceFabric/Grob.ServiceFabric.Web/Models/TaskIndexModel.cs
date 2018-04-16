using Grob.Entities.Docker;
using Grob.Entities.Grob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grob.ServiceFabric.Web.Models
{
    public class TaskIndexModel
    {
        public List<Container> Containers { get; set; }
        public List<GrobTask> Tasks { get; set; }
    }
}
