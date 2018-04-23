using Docker.DotNet;
using Docker.DotNet.Models;
using Grob.Entities.Docker;
using Grob.Entities.Grob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        public async Task CreateContainerAsync(GrobTask grobTask)
        {
            var parameters = new CreateContainerParameters()
            {
                Image = grobTask.ApplicationName,
                Name = grobTask.Name.ToLower().Replace(" ", string.Empty)
            };

            await _dockerClient.Containers.CreateContainerAsync(parameters);
        }

        public async Task CreateImageAsync(string workingDirectory, string name)
        {
            try
            {
                var processInfo = new ProcessStartInfo("cmd.exe", $"/c \"docker build -t {name} .\"")
                {
                    WindowStyle = ProcessWindowStyle.Normal,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    WorkingDirectory = workingDirectory
                };

                var process = new Process()
                {
                    StartInfo = processInfo
                };
                process.Start();
                process.WaitForExit();

                var output = process.StandardOutput.ReadToEnd();
            }
            catch(Exception e)
            {

            }            
        }

        public async Task CreateImageAsync(Stream contents, string dockerFilePath,string name)
        {
            var parameters = new ImageBuildParameters()
            {
                Dockerfile = dockerFilePath,
                Tags = new List<string>() { name.ToLower() },
                                
            };

            await _dockerClient.Images.BuildImageFromDockerfileAsync(contents, parameters);
        }



        public async Task<List<Container>> ListContainers()
        {
            var parameters = new ContainersListParameters()
            {
                All = true
            };

            var containers = await _dockerClient.Containers.ListContainersAsync(parameters);

            var result = new List<Container>();

            foreach (var container in containers)
            {
                result.Add(new Container(container.Command, container.Created, container.ID, container.Image, container.Names.FirstOrDefault(), container.Status));
            }

            return result;
        }

        public async Task<List<Application>> ListImagesAsync()
        {
            var parameters = new ImagesListParameters()
            {
                //All = true
            };

            var images = await _dockerClient.Images.ListImagesAsync(parameters);

            var result = new List<Application>();

            foreach (var image in images)
            {
                result.Add(new Application(image.RepoTags.FirstOrDefault(), image.Created, image.ID, image.Containers, image.Size));
            }

            return result;
        }

        public async Task StartContainerAsync(Container container)
        {
            await _dockerClient.Containers.StartContainerAsync(container.Id, new ContainerStartParameters());
        }
    }
}
