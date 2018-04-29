using Grob.Agent.Models;
using Grob.Entities.Docker;
using Grob.Entities.Grob;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

[assembly: FabricTransportServiceRemotingProvider(RemotingListener = RemotingListener.V2Listener, RemotingClient = RemotingClient.V2Client)]
namespace Grob.Master.Models
{
    public interface IGrobMasterService : IService
    {
        Task RunTaskAsync(GrobTask task);
        Task StopTask(GrobTask task);
        Task<List<Container>> GetContainersAsync();
        Task<List<GrobAgent>> GetGrobAgentsAsync();
        Task RegisterAgentAsync(GrobAgent grobAgent);
        Task<List<Application>> GetApplicationsAsync();
        Task<GrobTask> CreateContainerForTaskAsync(GrobTask grobTask);
        Task DeleteContainerForTaskAsync(GrobTask grobTask);
    }
}
