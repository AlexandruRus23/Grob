using Docker.DotNet;
using Docker.DotNet.Models;
using Grob.Docker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerAPIConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var dockerManager = new DockerManager();

			var client = new DockerManager();

			var containersList = client.ListContainers().Result;

			foreach(var container in containersList)
			{
				Console.WriteLine($"Image: {container.Image}. Tag: {container.Id}. Created At: {container.Created}");
			}

			var imagesList = client.ListImagesAsync().Result;

			foreach(var image in imagesList)
			{
				Console.WriteLine($"Image: {image.Name.FirstOrDefault()}. Created: {image.Created}");
			}

			Console.ReadKey();
		}
	}
}
