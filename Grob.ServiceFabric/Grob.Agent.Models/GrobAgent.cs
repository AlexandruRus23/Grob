using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Grob.Entities.Docker;
using Grob.Entities.Grob;
using Newtonsoft.Json;

namespace Grob.Agent.Models
{
    public class GrobAgent : IGrobAgentService
    {        
        public string Uri { get; set; }
        public string Name { get; set; }

        public GrobAgent(string name, string uri, long instanceId)
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
            var request = new HttpRequestMessage(HttpMethod.Get, Uri.ToString() + "containers");
            var result = client.SendAsync(request).Result;
            var containers = JsonConvert.DeserializeObject<List<Container>>(result.Content.ReadAsStringAsync().Result);

            return containers;
        }

        public void RunContainer(Container container)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, Uri.ToString() + $"containers/start")
            {
                Content = new StringContent(JsonConvert.SerializeObject(container), Encoding.UTF8, "application/json")
            };
            var result = client.SendAsync(request).Result;
        }

        public List<Application> GetApplications()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, Uri.ToString() + "applications");
            var result = client.SendAsync(request).Result;
            var applications = JsonConvert.DeserializeObject<List<Application>>(result.Content.ReadAsStringAsync().Result);

            return applications;
        }

        public void CreateContainers(GrobTask grobTask)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, Uri.ToString() + "containers")
            {
                Content = new StringContent(JsonConvert.SerializeObject(grobTask), Encoding.UTF8, "application/json")
            };
            var result = client.SendAsync(request).Result;
        }
    }
}
