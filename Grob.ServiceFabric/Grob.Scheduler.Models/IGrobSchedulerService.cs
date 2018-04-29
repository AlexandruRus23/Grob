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
        Task AddTaskAsync(GrobTask task);
        Task<List<GrobTask>> GetTasksAsync();
        Task DeleteTaskAsync(Guid taskId);
        Task StartTaskAsync(GrobTask grobTaskToRun);
    }
}
