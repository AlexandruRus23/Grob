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
        private const string JOBS_COLLECTION = "GrobJobs";
        private IReliableStateManager _stateManager;

        public ServiceFabricJobRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task AddJob(GrobJob job)
        {
            var grobJobs = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, GrobJob>>(JOBS_COLLECTION);

            using (var tx = _stateManager.CreateTransaction())
            {
                await grobJobs.AddOrUpdateAsync(tx, job.Id, job, (id, value) => job);

                await tx.CommitAsync();
            }
        }

        public async Task<IEnumerable<GrobJob>> GetJobs()
        {
            var jobs = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, GrobJob>>(JOBS_COLLECTION);
            var result = new List<GrobJob>();

            using (var tx = _stateManager.CreateTransaction())
            {
                var allJobs = await jobs.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (var enumerator = allJobs.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<Guid, GrobJob> current = enumerator.Current;
                        result.Add(current.Value);
                    }
                }
            }

            return result;
        }
    }
}
