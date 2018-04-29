using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grob.Entities.Grob;
using Grob.Scheduler.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace Grob.ServiceFabric.Web.Controllers
{
    [Produces("application/json")]
    public class GrobTaskRunnerController : Controller
    {
        private IGrobSchedulerService grobSchedulerService;

        public GrobTaskRunnerController()
        {
            grobSchedulerService = ServiceProxy.Create<IGrobSchedulerService>(new Uri("fabric:/Grob.ServiceFabric/Grob.ServiceFabric.Scheduler"), new ServicePartitionKey(1));
        }

        [Route("api/GrobTaskRunner"), HttpGet]
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [Route("api/GrobTaskRunner/{grobTaskName}"), HttpPost]
        public async Task<ActionResult> RunTask([FromRoute]string grobTaskName)
        {
            var tasks = await grobSchedulerService.GetTasksAsync();
            var grobTaskToRun = tasks.Where(t => t.Name == grobTaskName).FirstOrDefault();

            if (grobTaskToRun != null)
            {
                await grobSchedulerService.StartTaskAsync(grobTaskToRun);

                using (var client = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Post, grobTaskToRun.Url))
                    {
                        using (StreamReader streamReader = new StreamReader(Request.Body, Encoding.UTF8))
                        {
                            var requestContent = streamReader.ReadToEnd();
                            request.Content = new StringContent(requestContent, Encoding.UTF8, "application/json");                            

                            var response = await client.SendAsync(request);
                            var result = await response.Content.ReadAsStringAsync();
                            return Ok(result);
                        }
                    }
                }
            }

            return NotFound();
        }
    }
}