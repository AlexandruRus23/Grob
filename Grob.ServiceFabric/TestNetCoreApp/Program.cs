using Grob.Docker;
using System;
using System.Linq;

namespace TestNetCoreApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var dockerManager = new DockerManager();

            var containersList = dockerManager.ListContainers();

            foreach (var container in containersList)
            {
                Console.WriteLine($"Image: {container.Image}. Tag: {container.ID}. Created At: {container.Created}");
            }

            var imagesList = dockerManager.ListImages();

            foreach (var image in imagesList)
            {
                Console.WriteLine($"Image: {image.RepoTags.FirstOrDefault()}. Created: {image.Created}");
            }

            Console.ReadKey();
        }
    }
}
