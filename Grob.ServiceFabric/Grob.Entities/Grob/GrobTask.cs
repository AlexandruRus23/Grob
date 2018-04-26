using Grob.Entities.Docker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grob.Entities.Grob
{
    public class GrobTask
    {
        private Thread Runner;

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ApplicationName { get; set; }
        public ScheduleTypesEnum ScheduleType { get; set; }
        public string ScheduleInfo { get; set; }
        public DateTime CreationTime { get; set; }
        public string LastRunTime { get; set; }        
        public string NextRunTime { get; set; }

        public GrobTask()
        {
        }

        public GrobTask(string name, string applicationName, ScheduleTypesEnum scheduleType, string scheduleInfo) : this()
        {
            Id = Guid.NewGuid();
            Name = name;
            ApplicationName = applicationName;
            ScheduleType = scheduleType;
            ScheduleInfo = scheduleInfo;
            CreationTime = DateTime.Now;
            LastRunTime = "-1";
            NextRunTime = "-1";
        }
    }
}
