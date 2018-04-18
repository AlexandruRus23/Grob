using Grob.Entities.Grob;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using System.Collections.Generic;
using System.Threading.Tasks;

[assembly: FabricTransportServiceRemotingProvider(RemotingListener = RemotingListener.V2Listener, RemotingClient = RemotingClient.V2Client)]
namespace Grob.Scheduler.Models
{
    public interface IGrobSchedulerService : IService
    {
        Task<List<GrobTask>> GetTasksAsync();
    }
}
