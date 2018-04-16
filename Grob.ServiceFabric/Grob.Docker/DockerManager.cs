using Docker.DotNet;
using Docker.DotNet.Models;
using Grob.Entities.Docker;
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

        public async Task<List<Container>> ListContainers()
        {
            var parameters = new ContainersListParameters()
            {
                All = true
            };

            var containers = await _dockerClient.Containers.ListContainersAsync(parameters);

            var result = new List<Container>();

            foreach(var container in containers)
            {
                result.Add(new Container(container.Command, container.Created, container.ID, container.Image, container.Names.FirstOrDefault(), container.Status));
            }

            return result;
        }

        public async Task<IEnumerable<Image>> ListImages()
        {
            var parameters = new ImagesListParameters()
            {
                All = true
            };

            var images = await _dockerClient.Images.ListImagesAsync(parameters);

            var result = new List<Image>();

            foreach (var image in images)
            {
                result.Add(new Image(image.RepoTags, image.Created, image.ID, image.Containers, image.Size));
            }

            return result;
        }

        public async Task StartContainerAsync(Container container)
        {
            await _dockerClient.Containers.StartContainerAsync(container.Id, new ContainerStartParameters());
        }
    }
}
