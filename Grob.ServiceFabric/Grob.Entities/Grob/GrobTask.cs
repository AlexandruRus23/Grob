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
        public string Name { get; set; }
        public Guid Id { get; set; }

        public GrobTask()
        {

        }

        public GrobTask(string name)
        {
            Name = name;
        }
    }
}
