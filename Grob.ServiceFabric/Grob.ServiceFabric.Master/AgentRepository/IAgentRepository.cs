using Grob.Agent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grob.ServiceFabric.Master.AgentRepository
{
    public interface IGrobAgentRepository
    {
        Task AddAgent(GrobAgent grobAgent);
        Task<List<GrobAgent>> GetGrobAgentsAsync();
    }
}
