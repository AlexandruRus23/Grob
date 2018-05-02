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
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ApplicationName { get; set; }
        public ScheduleTypesEnum ScheduleType { get; set; }
        public string ScheduleInfo { get; set; }
        public DateTime CreationTime { get; set; }
        public string LastRunTime { get; set; }        
        public string NextRunTime { get; set; }
        public ContainerTypeEnum ContainerType { get; set; }
        public GrobTaskStatusEnum Status { get; set; }
        public string PrivateUrl { get; set; }
        public string PublicUrl { get; set; }
        public List<Container> Containers { get; set; }
        public List<GrobTaskLogs> Logs { get; set; }
        public int RequiredMemory { get; set; }

        public GrobTask()
        {
            Containers = new List<Container>();
            Logs = new List<GrobTaskLogs>();
        }

        public GrobTask(string name, string applicationName, ScheduleTypesEnum scheduleType, string scheduleInfo, ContainerTypeEnum containerType, int requiredMemory) : this()
        {
            Id = Guid.NewGuid();
            Name = name;
            ApplicationName = applicationName;
            ScheduleType = scheduleType;
            ScheduleInfo = scheduleInfo;
            CreationTime = DateTime.Now;
            LastRunTime = "-1";
            NextRunTime = "-1";
            ContainerType = containerType;
            RequiredMemory = requiredMemory;
        }
    }
}
