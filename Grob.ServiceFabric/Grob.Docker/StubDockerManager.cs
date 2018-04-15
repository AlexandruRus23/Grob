using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Docker.DotNet.Models;
using Grob.Entities.Docker;

namespace Grob.Docker
{
    public class StubDockerManager : IDockerManager
    {
        public async Task<IEnumerable<Container>> ListContainers()
        {
            return new List<Container>();
        }

        public async Task<IEnumerable<Image>> ListImages()
        {
            return new List<Image>();
        }
        

        public async Task StartContainerAsync(Container containerName)
        {
        }
    }
}
