using Grob.Docker;
using Grob.Entities.Docker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Grob.ServiceFabric.Agent.Controllers
{
    [Route("applications")]
    public class ApplicationsController : ApiController
    {
        private DockerManager _dockerManager;

        public ApplicationsController()
        {
            _dockerManager = new DockerManager();
        }

        [HttpGet]
        public async Task<List<Application>> GetImages()
        {
            return await _dockerManager.ListImagesAsync();
        }

        [HttpPost]
        [Route("")]
        public void CreateImage([FromBody]Stream stream, [FromUri] string name)
        {

        }
    }
}
