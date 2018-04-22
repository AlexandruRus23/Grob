using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grob.Entities.Docker
{
    public class Container
    {
        public string Command { get; set; }
        public DateTime Created { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string Status { get; set; }

        public Container(string command, DateTime created, string id, string image, string name, string status)
        {
            Command = command;
            Created = created;
            Id = id;
            Image = image;
            Name = name;
            Status = status;
        }

        public Container()
        {

        }

    }
}
