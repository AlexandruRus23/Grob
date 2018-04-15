﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Grob.Entities.Docker;

namespace Grob.Docker
{
    public interface IDockerManager
    {
        Task<IEnumerable<Container>> ListContainers();
        Task<IEnumerable<Image>> ListImages();
        Task StartContainerAsync(Container containerName);
    }
}