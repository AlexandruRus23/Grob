using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Docker.DotNet.Models;
using Grob.Entities.Docker;
using Grob.Entities.Grob;

namespace Grob.Docker
{
    public class StubDockerManager : IDockerManager
    {
        public async Task CreateContainerAsync(string path)
        {
            return;
        }

        public Task CreateContainerAsync(GrobTask grobTask)
        {
            throw new NotImplementedException();
        }

        public async Task CreateImageAsync(string workingDirectory, string name)
        {
            return;
        }

        public async Task CreateImageAsync(Stream contents, string dockerFilePath, string name)
        {
            return;
        }

        public async Task<List<Container>> ListContainers()
        {
            return new List<Container>();
        }

        public async Task<List<Application>> ListImagesAsync()
        {
            return new List<Application>();
        }        

        public async Task StartContainerAsync(Container containerName)
        {
            return;
        }
    }
}
