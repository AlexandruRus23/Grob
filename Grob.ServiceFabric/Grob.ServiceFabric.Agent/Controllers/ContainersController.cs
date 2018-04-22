namespace Grob.ServiceFabric.Agent.Controllers
{
    using Grob.Docker;
    using Grob.Entities.Docker;
    using Grob.Entities.Grob;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;

    [Route("containers")]
    public class ContainersController : ApiController
    {
        private DockerManager _dockerManager;

        public ContainersController()
        {
            _dockerManager = new DockerManager();
        }

        [HttpGet]
        public async Task<List<Container>> GetContainers()
        {
            return await _dockerManager.ListContainers();
        }

        [HttpPost]
        public async void CreateContainer([FromBody] GrobTask grobTask)
        {
            await _dockerManager.CreateContainerAsync(grobTask);
        }
    }
}
