using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Grob.Master.Models;
using Grob.ServiceFabric.Web.Models;
using Grob.ServiceFabric.Web.Models.Containers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace Grob.ServiceFabric.Web.Controllers
{
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
        public ActionResult Create(NewContainerModel model)
        {
            try
            {
                string archiveName = $"{Guid.NewGuid()}.zip";
                string archivePath = $"Archives/${archiveName}";
                FileStream fileStream = new FileStream(archivePath, FileMode.CreateNew);
                model.Archive.CopyTo(fileStream);
                fileStream.Close();
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
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