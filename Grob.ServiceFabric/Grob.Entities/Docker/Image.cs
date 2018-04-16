using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grob.Entities.Docker
{
    public class Image
    {
        public IList<string> RepoTags { get; set; }
        public DateTime Created { get; set; }
        public string ID { get; set; }
        public long Containers { get; set; }
        public long Size { get; set; }

        public Image(IList<string> repoTags, DateTime created, string iD, long containers, long size)
        {
            this.RepoTags = repoTags;
            this.Created = created;
            this.ID = iD;
            this.Containers = containers;
            this.Size = size;
        }

        public Image()
        {

        }
    }
}
