using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Grob.ServiceFabric.Web.Models.Containers
{
    public class NewContainerModel
    {
        public string Name { get; set; }
        public IFormFile Archive { get; set; }
    }
}
