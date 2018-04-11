using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grob.Docker
{
    public class DockerManager
    {
        private DockerClient _dockerClient;

        public DockerManager()
        {
            // local docker
            DockerClientConfiguration dockerClientConfiguration = new DockerClientConfiguration(new Uri(DockerSettings.DockerUri));
            _dockerClient = dockerClientConfiguration.CreateClient();
        }

        public IList<ContainerListResponse> ListContainers()
        {
            var parameters = new ContainersListParameters()
            {
                All = true
            };

            var containers = _dockerClient.Containers.ListContainersAsync(parameters).Result;

            return containers;
        }

        public IList<ImagesListResponse> ListImages()
        {
            var parameters = new ImagesListParameters()
            {
                All = true
            };

            var images = _dockerClient.Images.ListImagesAsync(parameters).Result;

            return images;
        }
    }
}
