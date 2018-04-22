using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grob.ServiceFabric.Web.Models.Applications
{
    public class NewApplicationViewModel
    {
        public string Name { get; set; }
        public IFormFile Archive { get; set; }
    }
}
