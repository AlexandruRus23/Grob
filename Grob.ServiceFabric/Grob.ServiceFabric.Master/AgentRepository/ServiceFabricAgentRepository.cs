using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grob.Agent.Models;
using Grob.Constants;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace Grob.ServiceFabric.Master.AgentRepository
{
    public class ServiceFabricAgentRepository : IAgentRepository
    {
        private IReliableStateManager _stateManager;

        public ServiceFabricAgentRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task AddAgent(GrobAgent grobAgent)
        {
            var agents = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, GrobAgent>>(RepositoryConstants.AGENT_REPOSITORY);

            using (var tx = _stateManager.CreateTransaction())
            {
                await agents.AddOrUpdateAsync(tx, grobAgent.AgentGuid, grobAgent, (id, value) => grobAgent);

                await tx.CommitAsync();
            }
        }

        public Task<List<GrobAgent>> GetGrobAgents()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Container>> GetAllContainersAsync()
        {
            var containers = await _stateManager.GetOrAddAsync<IReliableDictionary<string, Container>>(RepositoryConstants.CONTAINER_REPOSITORY);
            var result = new List<Container>();

            using (var tx = _stateManager.CreateTransaction())
            {
                var allContainers = await containers.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (var enumerator = allContainers.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<string, Container> current = enumerator.Current;
                        result.Add(current.Value);
                    }
                }
            }

            return result;
        }
    }
}
