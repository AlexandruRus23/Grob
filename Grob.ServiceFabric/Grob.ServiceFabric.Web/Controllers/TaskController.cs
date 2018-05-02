using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grob.Entities.Grob;
using Grob.Master.Models;
using Grob.Scheduler.Models;
using Grob.ServiceFabric.Web.Models;
using Grob.ServiceFabric.Web.Models.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace Grob.ServiceFabric.Web.Controllers
{
    public class TaskController : Controller
    {        
        private IGrobSchedulerService _grobSchedulerService;
        private IGrobMasterService _grobMasterService;

        public TaskController()
        {            
            _grobSchedulerService = ServiceProxy.Create<IGrobSchedulerService>(new Uri("fabric:/Grob.ServiceFabric/Grob.ServiceFabric.Scheduler"), new ServicePartitionKey(1));
            _grobMasterService = ServiceProxy.Create<IGrobMasterService>(new Uri("fabric:/Grob.ServiceFabric/Grob.ServiceFabric.Master"), new ServicePartitionKey(1));
        }

        // GET: Task
        public IActionResult Index()
        {
            var model = new TaskViewModel
            {
                Tasks = _grobSchedulerService.GetTasksAsync().Result.OrderBy(t => t.CreationTime).ToList()
            };

            return View("Index", model);
        }
        
        [Route("task/{taskName}"),HttpGet]
        public async Task<IActionResult> Details([FromRoute]string taskName)
        {
            var model = new TaskDetailsModel()
            {
                GrobTask = await _grobSchedulerService.GetTaskAsync(taskName)
            };

            return View("TaskDetails", model);
        }

        [Route("task/create"), HttpGet]
        // GET: Task/Create
        public IActionResult Create()
        {
            var model = new NewTaskModel();

            var applications = _grobMasterService.GetApplicationsAsync().Result;
            applications.ForEach(a => model.RegisteredApplications.Add(new SelectListItem()
            {
                Text = a.Name,
                Value = a.Name
            }));

            var scheduleTypes = Enum.GetValues(typeof(ScheduleTypesEnum)).Cast<ScheduleTypesEnum>().ToList();
            scheduleTypes.ForEach(s => model.ScheduleTypes.Add(new SelectListItem()
            {
                Text = s.ToString(),
                Value = s.ToString()
            }));

            var containerTypes = Enum.GetValues(typeof(ContainerTypeEnum)).Cast<ContainerTypeEnum>().ToList();
            containerTypes.ForEach(c => model.ContainerTypes.Add(new SelectListItem()
            {
                Text = c.ToString(),
                Value = c.ToString()
            }));

            return View(model);
        }

        // POST: Task/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NewTaskModel newTaskModel)
        {
            try
            {                
                var task = new GrobTask(newTaskModel.TaskName, newTaskModel.ApplicationName, newTaskModel.ScheduleType, newTaskModel.ScheduleInfo, newTaskModel.ContainerType, newTaskModel.RequiredMemory);
                task = await _grobMasterService.CreateContainerForTaskAsync(task);
                await _grobSchedulerService.CreateTaskAsync(task);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                return View();
            }
        }

        // POST: Task/Delete/5
        [Route("/delete/{taskId}"), HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid taskId)
        {
            try
            {
                await _grobSchedulerService.DeleteTaskAsync(taskId);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}