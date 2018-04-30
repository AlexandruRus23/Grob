namespace Grob.ServiceFabric.Agent.Controllers
{
    using Grob.Docker;
    using Grob.Entities.Docker;
    using Grob.Entities.Grob;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;

    
    public class ContainersController : ApiController
    {
        private DockerManager _dockerManager;

        public ContainersController()
        {
            _dockerManager = new DockerManager();
        }

        [Route("containers") , HttpGet]
        public async Task<List<Container>> GetContainers()
        {
            return await _dockerManager.ListContainers();
        }

        [Route("containers"), HttpPost]
        public async void CreateContainer([FromBody] GrobTask grobTask)
        {
            await _dockerManager.CreateContainerAsync(grobTask);
        }

        [Route("containers"), HttpDelete]
        public async void DeleteContainer([FromBody] GrobTask grobTask)
        {
            await _dockerManager.DeleteContainerAsync(grobTask);
        }

        [Route("containers/start"), HttpPost]
        public async Task<IHttpActionResult> RunContainer([FromBody] Container container)
        {
            try
            {
                await _dockerManager.StartContainerAsync(container);
                return Ok();
            }
            catch
            {
                return InternalServerError();
            }            
        }

        [Route("containers/stop"), HttpPost]
        public async void StopContainer([FromBody] Container container)
        {
            await _dockerManager.StopContainerAsync(container);
        }
    }
}
