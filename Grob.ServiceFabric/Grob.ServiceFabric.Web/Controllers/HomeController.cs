using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Grob.ServiceFabric.Web.Models;
using Grob.Master.Models;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Client;

namespace Grob.ServiceFabric.Web.Controllers
{
    public class HomeController : Controller
    {
        private IGrobMasterService _grobMasterService;

        public HomeController()
        {
            _grobMasterService = ServiceProxy.Create<IGrobMasterService>(new Uri("fabric:/Grob.ServiceFabric/Grob.ServiceFabric.Master"), new ServicePartitionKey(1));
        }

        public IActionResult Index()
        {
            var model = new IndexViewModel()
            {
                GrobAgents = _grobMasterService.GetGrobAgentsAsync().Result
            };

            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
