using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grob.Entities.Docker;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace Grob.ServiceFabric.Master.ContainerRepository
{
    public class ServiceFabricContainerRepository : IContainerRepository
    {
        private const string CONTAINER_COLLECTION = "GrobContainers";
        private IReliableStateManager _stateManager;

        public ServiceFabricContainerRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task AddContainerAsync(Container container)
        {
            var containers = await _stateManager.GetOrAddAsync<IReliableDictionary<string, Container>>(CONTAINER_COLLECTION);

            using (var tx = _stateManager.CreateTransaction())
            {
                await containers.AddOrUpdateAsync(tx, container.Id, container, (id, value) => container);

                await tx.CommitAsync();
            }
        }

        public async Task<IEnumerable<Container>> GetAllContainersAsync()
        {
            var containers = await _stateManager.GetOrAddAsync<IReliableDictionary<string, Container>>(CONTAINER_COLLECTION);
            var result = new List<Container>();

            using (var tx = _stateManager.CreateTransaction())
            {
                var allContainers = await containers.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (var enumerator = allContainers.GetAsyncEnumerator())
                {
                    while(await enumerator.MoveNextAsync(CancellationToken.None))
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
