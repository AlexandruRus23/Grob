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

        public async Task AddRunnerAsync(BaseScheduleRunner runner)
        {
            var runners = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, BaseScheduleRunner>>(RepositoryConstants.RUNNERS_REPOSITORY);

            using (var tx = _stateManager.CreateTransaction())
            {
                await runners.AddOrUpdateAsync(tx, runner.Id, runner, (id, value) => runner);

                await tx.CommitAsync();
            }
        }

        public async Task StopRunner(Guid runnerId)
        {
            var runners = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, BaseScheduleRunner>>(RepositoryConstants.RUNNERS_REPOSITORY);

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

        public async Task<List<BaseScheduleRunner>> GetRunners()
        {
            var runners = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, BaseScheduleRunner>>(RepositoryConstants.RUNNERS_REPOSITORY);
            var result = new List<BaseScheduleRunner>();

            using (var tx = _stateManager.CreateTransaction())
            {
                var allRunners = await runners.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (var enumerator = allRunners.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<Guid, BaseScheduleRunner> current = enumerator.Current;
                        result.Add(current.Value);
                    }
                }
            }

            return result;
        }

        public Task<BaseScheduleRunner> GetRunner(Guid grobTask)
        {
            throw new NotImplementedException();
        }
    }
}
