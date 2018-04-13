using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grob.Entities.Grob
{
    public class Job
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public string JobImageName { get; set; }
        public string JobImageTag { get; set; }

        public Job()
        {

        }
        public Job(string name, string jobImageName, string jobImageTag)
        {
            Id = Guid.NewGuid();
            Name = name;
            CreationDate = DateTime.Now;
            JobImageName = jobImageName;
            JobImageTag = jobImageTag;
        }

        
    }
}
