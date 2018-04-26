using Grob.Agent.Models;
using Grob.Entities.Docker;
using Grob.Entities.Grob;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

[assembly: FabricTransportServiceRemotingProvider(RemotingListener = RemotingListener.V2Listener, RemotingClient = RemotingClient.V2Client)]
namespace Grob.Master.Models
{
    public interface IGrobMasterService : IService
    {
        Task RunTask(GrobTask task);
        Task<List<Container>> GetContainersAsync();
        Task<List<GrobAgentHttpClient>> GetGrobAgentsAsync();
        Task RegisterAgentAsync(GrobAgentHttpClient grobAgent);
        Task<List<Application>> GetApplicationsAsync();
        Task CreateContainerForTaskAsync(GrobTask grobTask);
        Task DeleteContainerForTaskAsync(GrobTask grobTask);
    }
}
