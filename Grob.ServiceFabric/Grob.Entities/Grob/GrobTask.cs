using Grob.Entities.Docker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grob.Entities.Grob
{
    public class GrobTask
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ContainerName { get; set; }

        public GrobTask()
        {

        }

        public GrobTask(string name, string containerName)
        {
            Name = name;
            ContainerName = containerName;
            Id = Guid.NewGuid();
        }
    }
}
