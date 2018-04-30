using Grob.Agent.Models;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grob.ServiceFabric.Agent
{
    public static class AgentResourceUtilization
    {
        private static Queue<string> _cpuUsage = new Queue<string>();
        private static Queue<string> _availableMemory = new Queue<string>();
        private static ServiceController dockerEngineServiceController = new ServiceController("docker");
        private static ServiceController dockerForWindowsServiceController = new ServiceController("com.docker.service");

        public static void AddAgentInformation(AgentInformation agentInformation)
        {
            _cpuUsage.Enqueue(agentInformation.CpuUsage);
            _availableMemory.Enqueue(agentInformation.AvailableMemory);

            if (_cpuUsage.Count > 5)
            {
                _cpuUsage.Dequeue();
                _availableMemory.Dequeue();
            }
        }

        public static AgentInformation GetAgentInformation()
        {
            dockerEngineServiceController.Refresh();
            dockerForWindowsServiceController.Refresh();

            return new AgentInformation()
            {
                IsDockerEngineRunning = dockerEngineServiceController.Status == ServiceControllerStatus.Running,
                IsDockerForWindowsServiceRunning = dockerForWindowsServiceController.Status == ServiceControllerStatus.Running,
                CpuUsage = GetAverageCPUUsage(_cpuUsage).Length > 5 ? GetAverageCPUUsage(_cpuUsage).Substring(0, 5) : GetAverageCPUUsage(_cpuUsage),
                AvailableMemory = GetAverageAvailableMemory(_availableMemory)
            };
        }

        private static string GetAverageCPUUsage(Queue<string> cpuUsage)
        {
            float sum = 0;

            foreach (var entry in cpuUsage)
            {
                float.TryParse(entry, out float value);
                sum += value;
            }

            return (sum / cpuUsage.Count).ToString();
        }

        private static string GetAverageAvailableMemory(Queue<string> availableMemory)
        {
            if(availableMemory.Count > 0)
            {
                int sum = 0;

                foreach (var entry in availableMemory)
                {
                    Int32.TryParse(entry, out int value);
                    sum += value;
                }

                return (sum / availableMemory.Count).ToString();
            }

            return "N/A";            
        }
    }
}
