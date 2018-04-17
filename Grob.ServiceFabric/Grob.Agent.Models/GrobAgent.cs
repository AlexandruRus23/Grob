using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Grob.Entities.Docker;
using Newtonsoft.Json;

namespace Grob.Agent.Models
{
    public class GrobAgent : IGrobAgentService
    {        
        public string Uri { get; set; }
        public string Name { get; set; }

        public GrobAgent(string name, string uri)
        {
            Name = name;
            Uri = uri;
        }

        public GrobAgent()
        {
        }

        public List<Container> GetContainers()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, Uri.ToString() + "container");
            var result = client.SendAsync(request).Result;
            var containers = JsonConvert.DeserializeObject<List<Container>>(result.Content.ReadAsStringAsync().Result);

            return containers;
        }

        public void RunContainer(Container container)
        {
            throw new NotImplementedException();
        }
    }
}
