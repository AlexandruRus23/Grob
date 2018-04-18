using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grob.Agent.Models;
using Grob.Constants;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace Grob.ServiceFabric.Master.AgentRepository
{
    public class ServiceFabricAgentRepository : IGrobAgentRepository
    {
        private IReliableStateManager _stateManager;

        public ServiceFabricAgentRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task AddAgent(GrobAgent grobAgent)
        {
            var agents = await _stateManager.GetOrAddAsync<IReliableDictionary<string, GrobAgent>>(RepositoryConstants.AGENT_REPOSITORY);

            using (var tx = _stateManager.CreateTransaction())
            {
                await agents.AddOrUpdateAsync(tx, grobAgent.Name, grobAgent, (id, value) => grobAgent);

                await tx.CommitAsync();
            }
        }

        public async Task<List<GrobAgent>> GetGrobAgentsAsync()
        {
            var agents = await _stateManager.GetOrAddAsync<IReliableDictionary<string, GrobAgent>>(RepositoryConstants.AGENT_REPOSITORY);
            var result = new List<GrobAgent>();

            using (var tx = _stateManager.CreateTransaction())
            {
                var allContainers = await agents.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (var enumerator = allContainers.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<string, GrobAgent> current = enumerator.Current;
                        result.Add(current.Value);
                    }
                }
            }

            return result;
        }
    }
}
