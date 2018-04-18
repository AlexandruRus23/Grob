using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grob.Master.Models;
using Grob.ServiceFabric.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace Grob.ServiceFabric.Web.Controllers
{
    [Route("container")]
    public class ContainerController : Controller
    {
        private IGrobMasterService _grobMasterService;

        public ContainerController()
        {
            _grobMasterService = ServiceProxy.Create<IGrobMasterService>(new Uri("fabric:/Grob.ServiceFabric/Grob.ServiceFabric.Master"), new ServicePartitionKey(1));
        }

        [HttpGet]
        // GET: Container
        public ActionResult Index()
        {
            var model = new ContainerViewModel()
            {
                Containers = _grobMasterService.GetContainersAsync().Result
            };

            return View(model);
        }

        // GET: Container/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Container/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Container/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Container/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Container/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Container/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Container/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}