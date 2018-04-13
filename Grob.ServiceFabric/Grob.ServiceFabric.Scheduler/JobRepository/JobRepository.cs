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

        public async Task AddJob(Job job)
        {
            var grobJobs = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Job>>(JOBS_COLLECTION);

            using (var tx = _stateManager.CreateTransaction())
            {
                await grobJobs.AddOrUpdateAsync(tx, job.Id, job, (id, value) => job);

                await tx.CommitAsync();
            }
        }

        public async Task<IEnumerable<Job>> GetJobs()
        {
            var jobs = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Job>>(JOBS_COLLECTION);
            var result = new List<Job>();

            using (var tx = _stateManager.CreateTransaction())
            {
                var allJobs = await jobs.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (var enumerator = allJobs.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<Guid, Job> current = enumerator.Current;
                        result.Add(current.Value);
                    }
                }
            }

            return result;
        }
    }
}
