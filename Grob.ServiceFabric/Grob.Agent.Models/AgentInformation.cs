using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grob.Agent.Models
{
    public class AgentInformation
    {
        public string CpuUsage { get; set; }
        public string AvailableMemory { get; set; }
        public bool IsDockerEngineRunning { get; set; }
        public bool IsDockerForWindowsServiceRunning { get; set; }
    }
}
