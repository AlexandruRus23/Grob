using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Grob.Agent.Models
{
    public class GrobAgent
    {        
        public Uri Uri { get; set; }
        public string Name { get; set; }

        public GrobAgent(string name, Uri uri)
        {
            Name = name;
            Uri = uri;
        }
    }
}
