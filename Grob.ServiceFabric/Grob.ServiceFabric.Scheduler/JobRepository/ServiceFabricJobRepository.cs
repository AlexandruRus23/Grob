using System;
using System.Collections.Generic;
using System.Fabric;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grob.Entities.Grob;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace Grob.ServiceFabric.Scheduler.JobRepository
{
    public class ServiceFabricJobRepository : IJobRepository
    {
        private const string TASK_COLLECTION = "GrobJobs";
        private IReliableStateManager _stateManager;

        public ServiceFabricJobRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task AddTask(GrobTask task)
        {
            var grobJobs = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, GrobTask>>(TASK_COLLECTION);

            using (var tx = _stateManager.CreateTransaction())
            {
                await grobJobs.AddOrUpdateAsync(tx, task.Id, task, (id, value) => task);

                await tx.CommitAsync();
            }
        }

        public async Task<IEnumerable<GrobTask>> GetTasks()
        {
            var tasks = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, GrobTask>>(TASK_COLLECTION);
            var result = new List<GrobTask>();

            using (var tx = _stateManager.CreateTransaction())
            {
                var allTasks = await tasks.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (var enumerator = allTasks.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<Guid, GrobTask> current = enumerator.Current;
                        result.Add(current.Value);
                    }
                }
            }

            return result;
        }
    }
}
