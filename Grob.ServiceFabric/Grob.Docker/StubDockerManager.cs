using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Docker.DotNet.Models;

namespace Grob.Docker
{
    public class StubDockerManager : IDockerManager
    {
        public async Task<IList<ContainerListResponse>> ListContainers()
        {
            return null;
        }

        public async Task<IList<ImagesListResponse>> ListImages()
        {
            return null;
        }

        public async Task RunImage(string imageName)
        {            
        }

        public async Task StartContainer(string containerName)
        {            
        }
    }
}
