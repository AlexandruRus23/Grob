using Grob.Entities.Docker;
using System.Collections.Generic;

namespace Grob.Agent.Models
{        
    public interface IGrobAgentService 
    {
        void RunContainer(Container container);
        List<Container> GetContainers();
    }
}
