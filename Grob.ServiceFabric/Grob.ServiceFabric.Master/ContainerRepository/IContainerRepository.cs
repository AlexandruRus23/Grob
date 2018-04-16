using Grob.Entities.Docker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grob.ServiceFabric.Master.ContainerRepository
{
    interface IContainerRepository
    {
        Task AddContainerAsync(Container container);
        Task<List<Container>> GetAllContainersAsync();
    }
}
