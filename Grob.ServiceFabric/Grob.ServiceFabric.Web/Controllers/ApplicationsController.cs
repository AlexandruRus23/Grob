﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grob.Docker;
using Grob.Entities.Grob;
using Grob.Master.Models;
using Grob.ServiceFabric.Web.Models.Applications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace Grob.ServiceFabric.Web.Controllers
{
    public class ApplicationsController : Controller
    {
        private IGrobMasterService _grobMasterService;
        private DockerManager _dockerManager;

        public ApplicationsController()
        {
            _grobMasterService = ServiceProxy.Create<IGrobMasterService>(new Uri("fabric:/Grob.ServiceFabric/Grob.ServiceFabric.Master"), new ServicePartitionKey(1));
            _dockerManager = new DockerManager();
        }

        // GET: Applications
        public async Task<ActionResult> Index()
        {
            var model = new ApplicationsIndexModel()
            {
                Applications = await _grobMasterService.GetApplicationsAsync()
            };

            return View(model);
        }

        // GET: Applications/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Applications/Create
        public ActionResult Create()
        {
            var model = new NewApplicationViewModel();
            
            var applicationTypes = Enum.GetValues(typeof(ApplicationTypeEnum)).Cast<ApplicationTypeEnum>().ToList();

            applicationTypes.ForEach(type => model.ApplicationTypes.Add(new SelectListItem()
            {
                Value = type.ToString(),
                Text = type.ToString()
            }));

            return View(model);
        }

        // POST: Applications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NewApplicationViewModel model)
        {
            var guid = Guid.NewGuid();
            string archiveName = $"{model.Name}.zip";
            string containerPath = $"Archives/{guid}";

            try
            {
                string archivePath = $"{containerPath}/{archiveName}";
                string extractPath = $"{containerPath}/extractionFoler";
                Directory.CreateDirectory(containerPath);

                using (var fileStream = new FileStream(archivePath, FileMode.CreateNew))
                {
                    model.Archive.CopyTo(fileStream);
                }

                ZipFile.ExtractToDirectory(archivePath, extractPath);

                CreateDockerFile(extractPath, model.Name, model.ApplicationType);
                await _dockerManager.CreateImageAsync($"{Directory.GetCurrentDirectory()}/{extractPath}", model.Name.ToLower());

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
            finally
            {
                Directory.Delete(containerPath, true);
            }
        }

        private void CreateDockerFile(string extractionPath, string entryPointFile, ApplicationTypeEnum applicationType)
        {
            var dockerFilePath = $"{extractionPath}/Dockerfile";

            //System.IO.File.Create(dockerFilePath);
            var content = new StringBuilder();

            switch (applicationType)
            {
                case ApplicationTypeEnum.DotnetFramework:
                    content.AppendLine("FROM microsoft/windowsservercore");
                    content.AppendLine("ADD . grobPackage");
                    content.AppendLine($"ENTRYPOINT [\"C:\\\\grobPackage\\\\{entryPointFile}.exe\"]");
                    break;
                case ApplicationTypeEnum.AspNetCore:
                    content.AppendLine("FROM microsoft/aspnetcore");
                    content.AppendLine("ADD . grobPackage");
                    content.AppendLine("WORKDIR /grobPackage");
                    content.AppendLine($"ENTRYPOINT [\"dotnet\", \"{entryPointFile}.dll\"]");
                    break;
            }
            

            using (var streamWriter = new StreamWriter(dockerFilePath))
            {
                streamWriter.Write(content.ToString());
            }
        }

        // GET: Applications/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Applications/Edit/5
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

        // GET: Applications/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Applications/Delete/5
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