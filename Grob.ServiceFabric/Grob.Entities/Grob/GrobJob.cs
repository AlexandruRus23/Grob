using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grob.Entities.Grob
{
    public class GrobJob
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public string JobImageName { get; set; }
        public string JobImageTag { get; set; }

        public GrobJob()
        {

        }
        public GrobJob(string name, string jobImageName, string jobImageTag)
        {
            Id = Guid.NewGuid();
            Name = name;
            CreationDate = DateTime.Now;
            JobImageName = jobImageName;
            JobImageTag = jobImageTag;
        }

        
    }
}
