using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Grob.Entities.Docker;
using Grob.Entities.Grob;

namespace Grob.Docker
{
    public interface IDockerManager
    {
        Task<List<Container>> ListContainers();
        Task<List<Application>> ListImagesAsync();
        Task StartContainerAsync(Container containerName);
        Task StopContainerAsync(Container containerName);
        Task CreateImageAsync(string workingDirectory, string name);
        Task CreateImageAsync(Stream contents, string dockerFilePath, string name);
        Task CreateContainerAsync(GrobTask grobTask);
        Task DeleteContainerAsync(GrobTask grobTask);
        Task<string> GetLogsForTaskAsync(GrobTask grobTask);
    }
}