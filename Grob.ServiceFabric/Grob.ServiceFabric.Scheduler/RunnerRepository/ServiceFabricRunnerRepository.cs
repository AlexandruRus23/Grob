using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grob.Constants;
using Grob.Entities.Grob;
using Grob.ServiceFabric.Scheduler.Schedule;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace Grob.ServiceFabric.Scheduler.RunnerRepository
{
    public class ServiceFabricRunnerRepository : IRunnerRepository
    {
        private IReliableStateManager _stateManager;

        public ServiceFabricRunnerRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task AddRunnerAsync(IScheduleRunner runner)
        {
            var runners = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, IScheduleRunner>>(RepositoryConstants.RUNNERS_REPOSITORY);

            using (var tx = _stateManager.CreateTransaction())
            {
                await runners.AddOrUpdateAsync(tx, runner.Id, runner, (id, value) => runner);

                await tx.CommitAsync();
            }
        }

        public async Task StopRunner(Guid runnerId)
        {
            var runners = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, IScheduleRunner>>(RepositoryConstants.RUNNERS_REPOSITORY);

            using (var tx = _stateManager.CreateTransaction())
            {
                var runner = runners.TryGetValueAsync(tx, runnerId).Result.Value;

                runner?.Stop();
            }                

            using (var tx = _stateManager.CreateTransaction())
            {
                await runners.TryRemoveAsync(tx, runnerId);

                await tx.CommitAsync();
            }
        }

        public async Task<List<IScheduleRunner>> GetRunners()
        {
            var runners = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, IScheduleRunner>>(RepositoryConstants.RUNNERS_REPOSITORY);
            var result = new List<IScheduleRunner>();

            using (var tx = _stateManager.CreateTransaction())
            {
                var allRunners = await runners.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (var enumerator = allRunners.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<Guid, IScheduleRunner> current = enumerator.Current;
                        result.Add(current.Value);
                    }
                }
            }

            return result;
        }

        public Task<IScheduleRunner> GetRunner(Guid grobTask)
        {
            throw new NotImplementedException();
        }
    }
}
