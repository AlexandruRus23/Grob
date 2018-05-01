﻿using Grob.Entities.Docker;
using Grob.Entities.Grob;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Grob.Agent.Models
{        
    public interface IGrobAgentService 
    {
        Task<bool> RunContainerAsync(Container container);
        Task<string> GetLogsForTaskAsync(GrobTask task);
        void StopContainer(Container container);
        List<Container> GetContainers();
        List<Application> GetApplications();
        void CreateContainers(GrobTask grobTask);
        void DeleteContainers(GrobTask grobTask);
        AgentInformation GetAgentInformation();
    }
}
