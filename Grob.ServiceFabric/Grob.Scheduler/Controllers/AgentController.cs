using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grob.ServiceFabric.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace Grob.Scheduler.Controllers
{
    [Produces("application/json")]
    [Route("api/Agent")]
    public class AgentController : Controller
    {
        private IReliableStateManager _statemanager;
        //private IReliableDictionary _agentList;

        public AgentController(IReliableStateManager stateManager)
        {
            _statemanager = stateManager;
          //  _agentList = new 
        }

        // GET: api/Agent
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Agent/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Agent
        [HttpPost]
        public void Post([FromBody]GrobAgent value)
        {           
        }
                
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
