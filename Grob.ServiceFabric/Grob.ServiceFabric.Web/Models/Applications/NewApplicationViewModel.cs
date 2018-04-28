using Grob.Entities.Grob;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public ApplicationTypeEnum ApplicationType { get; set; }
        public List<SelectListItem> ApplicationTypes { get; set; }       

        public NewApplicationViewModel()
        {
            ApplicationTypes = new List<SelectListItem>();
        }
    }
}
