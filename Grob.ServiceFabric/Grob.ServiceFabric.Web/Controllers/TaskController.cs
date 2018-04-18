﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grob.Master.Models;
using Grob.Scheduler.Models;
using Grob.ServiceFabric.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace Grob.ServiceFabric.Web.Controllers
{
    public class TaskController : Controller
    {        
        private IGrobSchedulerService _grobSchedulerService;

        public TaskController()
        {            
            _grobSchedulerService = ServiceProxy.Create<IGrobSchedulerService>(new Uri("fabric:/Grob.ServiceFabric/Grob.ServiceFabric.Scheduler"), new ServicePartitionKey(1));
        }

        // GET: Task
        public IActionResult Index()
        {
            var model = new TaskViewModel
            {
                Tasks = _grobSchedulerService.GetTasksAsync().Result
            };

            return View("Index", model);
        }

        // GET: Task/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: Task/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Task/Create
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

        // GET: Task/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Task/Edit/5
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

        // GET: Task/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Task/Delete/5
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