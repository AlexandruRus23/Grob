using Grob.Entities.Grob;
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
        public ScheduleTypesEnum ScheduleType { get; set; }
        public string ScheduleInfo { get; set; }
        public List<SelectListItem> ScheduleTypes { get; set; }
        public ContainerTypeEnum ContainerType { get; set; }
        public List<SelectListItem> ContainerTypes { get; set; } 
        public List<SelectListItem> RegisteredApplications { get; set; }
        

        public NewTaskModel()
        {
            RegisteredApplications = new List<SelectListItem>();
            ScheduleTypes = new List<SelectListItem>();
            ContainerTypes = new List<SelectListItem>();
        }
    }
}
