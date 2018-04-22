using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grob.ServiceFabric.Web.Models.Tasks
{
    public class NewTaskModel
    {
        public string TaskName { get; set; }
        public string ApplicationName { get; set; }
        public List<SelectListItem> RegisteredApplications { get; set; }

        public NewTaskModel()
        {
            RegisteredApplications = new List<SelectListItem>();
        }
    }
}
