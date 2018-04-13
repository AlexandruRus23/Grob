using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grob.Docker
{
    public class DockerManager : IDockerManager
    {
        private DockerClient _dockerClient;

        public DockerManager()
        {
            // local docker
            DockerClientConfiguration dockerClientConfiguration = new DockerClientConfiguration(new Uri(DockerSettings.DockerUri));
            _dockerClient = dockerClientConfiguration.CreateClient();
        }

        public async Task<IList<ContainerListResponse>> ListContainers()
        {
            var parameters = new ContainersListParameters()
            {
                All = true
            };

            var containers = await _dockerClient.Containers.ListContainersAsync(parameters);

            return containers;
        }

        public async Task<IList<ImagesListResponse>> ListImages()
        {
            var parameters = new ImagesListParameters()
            {
                All = true
            };

            var images = await _dockerClient.Images.ListImagesAsync(parameters);

            return images;
        }

        public async Task RunImage(string imageName)
        {
            //_dockerClient.Containers.
        }

        public async Task StartContainer(string containerName)
        {
            await _dockerClient.Containers.StartContainerAsync(containerName, new ContainerStartParameters());
        }
    }
}
