using System.Collections.Generic;
using System.Threading.Tasks;
using Docker.DotNet.Models;

namespace Grob.Docker
{
    public interface IDockerManager
    {
        Task<IList<ContainerListResponse>> ListContainers();
        Task<IList<ImagesListResponse>> ListImages();
        Task RunImage(string imageName);
        Task StartContainer(string containerName);
    }
}