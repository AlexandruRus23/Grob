using DemoWebApplication;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace WebApiTriggerCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Started at {DateTime.Now}");

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, "http://172.31.16.1:8080/api/GrobTaskRunner/api1"))
                {
                    var user = new User()
                    {
                        Username = "Alexandru"
                    };
                    client.Timeout = new TimeSpan(0, 2, 0);
                    Console.WriteLine($"Reqest Uri: {request.RequestUri}");
                    request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                    var response = client.SendAsync(request).Result;
                    var contents = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"Received: {contents}. At {DateTime.Now}");
                }
            }
        }
    }
}
