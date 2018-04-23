using Grob.Entities.Docker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grob.Entities.Grob
{
    public class GrobTask
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ApplicationName { get; set; }
        public ScheduleTypesEnum ScheduleType { get; set; }
        public string ScheduleInfo { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastRunTime { get; set; }

        public GrobTask()
        {

        }

        public GrobTask(string name, string applicationName, ScheduleTypesEnum scheduleType, string scheduleInfo)
        {
            Id = Guid.NewGuid();
            Name = name;
            ApplicationName = applicationName;
            ScheduleType = scheduleType;
            ScheduleInfo = scheduleInfo;
            CreationTime = DateTime.Now;
        }
    }
}
