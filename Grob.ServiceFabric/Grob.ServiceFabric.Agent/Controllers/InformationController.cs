using Grob.Agent.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Grob.ServiceFabric.Agent.Controllers
{
    public class InformationController : ApiController
    {
        public InformationController()
        {
        }

        [Route("information"), HttpGet]
        public AgentInformation GetInformation()
        {
            return AgentResourceUtilization.GetAgentInformation();
        }
    }
}
