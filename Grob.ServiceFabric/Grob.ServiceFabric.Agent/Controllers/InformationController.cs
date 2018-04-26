using Grob.Agent.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Grob.ServiceFabric.Agent.Controllers
{
    public class InformationController : ApiController
    {
        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;

        public InformationController()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        [Route("information"), HttpGet]
        public AgentInformation GetInformation()
        {
            var information = new AgentInformation()
            {
                CpuUsage = $"{cpuCounter.NextValue()}%",
                MemoryUsage = $"{ramCounter.NextValue()}%"
            };

            return information;
        }
    }
}
