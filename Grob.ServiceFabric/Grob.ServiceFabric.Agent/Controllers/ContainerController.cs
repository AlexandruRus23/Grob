namespace Grob.ServiceFabric.Agent.Controllers
{
    using Grob.Docker;
    using Grob.Entities.Docker;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;

    [Route("container")]
    public class ContainerController : ApiController
    {
        private DockerManager _dockerManager;

        public ContainerController()
        {
            _dockerManager = new DockerManager();
        }

        [HttpGet]
        public async Task<List<Container>> GetContainers()
        {
            return await _dockerManager.ListContainers();
        }
    }
}
