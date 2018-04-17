﻿using Docker.DotNet.Models;
using Grob.Entities.Docker;
using Grob.Entities.Grob;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FabricTransportServiceRemotingProvider(RemotingListener = RemotingListener.V2Listener, RemotingClient = RemotingClient.V2Client)]
namespace Grob.Agent.Models
{        
    public interface IGrobAgentService : IService
    {
        void RunContainer(Container container);
        List<Container> GetContainers();
    }
}
