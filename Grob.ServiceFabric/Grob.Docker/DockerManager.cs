using Docker.DotNet;
using Docker.DotNet.Models;
using Grob.Entities.Docker;
using Grob.Entities.Grob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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
            var parameters = GetCreateParameters(grobTask);
            await _dockerClient.Containers.CreateContainerAsync(parameters);
        }

        private CreateContainerParameters GetCreateParameters(GrobTask grobTask)
        {
            var parameters = new CreateContainerParameters()
            {
                Image = grobTask.ApplicationName,
                Name = GetContainerName(grobTask),
            };

            switch (grobTask.ContainerType)
            {
                case ContainerTypeEnum.Executable:
                    return parameters;
                case ContainerTypeEnum.WebApplication:
                    string containerPort = "80";
                    Uri hostUri = new Uri(grobTask.PrivateUrl);
                    string hostPort = hostUri.Port.ToString();

                    parameters.ExposedPorts = new Dictionary<string, EmptyStruct>()
                    {
                        { containerPort, new EmptyStruct() }
                    };
                    parameters.HostConfig = new HostConfig()
                    {
                        PortBindings = new Dictionary<string, IList<PortBinding>>()
                        {
                            {
                                containerPort,
                                new List<PortBinding>()
                                {
                                    new PortBinding() { HostPort = hostPort }
                                }
                            }
                        }
                    };
                    
                    break;
            }

            return parameters;
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
            catch (Exception e)
            {

            }
        }

        public async Task CreateImageAsync(Stream contents, string dockerFilePath, string name)
        {
            var parameters = new ImageBuildParameters()
            {
                Dockerfile = dockerFilePath,
                Tags = new List<string>() { name.ToLower() },

            };

            await _dockerClient.Images.BuildImageFromDockerfileAsync(contents, parameters);
        }

        public async Task DeleteContainerAsync(GrobTask grobTask)
        {
            var containers = await ListContainers();
            var containerToDelete = containers.Where(c => c.Name == GetContainerName(grobTask)).FirstOrDefault();
            if (containerToDelete != null)
            {
                await _dockerClient.Containers.StopContainerAsync(containerToDelete.Id, new ContainerStopParameters());

                await _dockerClient.Containers.RemoveContainerAsync(containerToDelete.Id, new ContainerRemoveParameters());
            }
        }

        public async Task<List<Container>> ListContainers()
        {
            var result = new List<Container>();
            var parameters = new ContainersListParameters()
            {
                All = true
            };

            try
            {
                var containers = await _dockerClient.Containers.ListContainersAsync(parameters);
                foreach (var container in containers)
                {
                    result.Add(new Container(container.Command, container.Created, container.ID, container.Image, container.Names.FirstOrDefault().Substring(1), container.Status));
                }
            }
            catch
            {

            }            

            return result;
        }

        public async Task<List<Application>> ListImagesAsync()
        {
            var result = new List<Application>();
            var parameters = new ImagesListParameters()
            {
                //All = true
            };

            try
            {
                var images = await _dockerClient.Images.ListImagesAsync(parameters);

                foreach (var image in images)
                {
                    result.Add(new Application(image.RepoTags.FirstOrDefault(), image.Created, image.ID, image.Containers, image.Size));
                }
            }
            catch (Exception e)
            {
            }            

            return result;
        }

        public async Task StartContainerAsync(Container container)
        {
            await _dockerClient.Containers.StartContainerAsync(container.Id, new ContainerStartParameters());
            var allContainers = await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters());
            var startedContainer = allContainers.Where(c => c.ID == container.Id).FirstOrDefault();
            if(startedContainer.Ports.Count > 0)
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    while (true)
                        try
                        {
                            tcpClient.Connect("127.0.0.1", startedContainer.Ports.FirstOrDefault().PublicPort);
                            break;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Port closed");
                        }
                }
            }            
        }

        private string GetContainerName(GrobTask grobTask)
        {
            return grobTask.Name.ToLower().Replace(" ", string.Empty);
        }

        public async Task StopContainerAsync(Container container)
        {
            await _dockerClient.Containers.StopContainerAsync(container.Id, new ContainerStopParameters());
        }

        public async Task<string> GetLogsForTaskAsync(GrobTask grobTask)
        {
            var containerName = GetContainerName(grobTask);
            var container = ListContainers().Result.Where(c => c.Name == containerName).FirstOrDefault();

            var logs = await _dockerClient.Containers.GetContainerLogsAsync(container.Id, new ContainerLogsParameters()
            {
                Since = grobTask.LastRunTime.ToString()
            });

            using (var streamReader = new StreamReader(logs))
            {
                var content = streamReader.ReadToEnd();
                return content;
            }
        }
    }
}
