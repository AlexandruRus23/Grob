using Grob.Entities.Grob;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[assembly: FabricTransportServiceRemotingProvider(RemotingListener = RemotingListener.V2Listener, RemotingClient = RemotingClient.V2Client)]
namespace Grob.Scheduler.Models
{
    public interface IGrobSchedulerService : IService
    {
        Task CreateTaskAsync(GrobTask task);
        Task<List<GrobTask>> GetTasksAsync();
        Task<GrobTask> GetTaskAsync(string taskName);
        Task DeleteTaskAsync(Guid taskId);
        Task<string> StartTaskAsync(GrobTask grobTaskToRun);
    }
}
