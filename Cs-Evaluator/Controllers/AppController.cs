using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cs_Evaluator.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cs_Evaluator.Controllers
{
    public class AppController : Controller
    {
        private IHostingEnvironment hostingEnv;

        public AppController(IHostingEnvironment env)
        {
            this.hostingEnv = env;
        }

        public IActionResult Index()
        {
            ViewData["Message"] = "CsEvaluator: Pagina principala";
            return View();

        }

        [HttpPost]
        public IActionResult Index(HomeworkViewModel model, IList<IFormFile> files)
        {

            long size = 0;
            try
            {
                var filename = ContentDispositionHeaderValue
                    .Parse(model.CsProject.ContentDisposition)
                    .FileName
                    .Trim('"');
                filename = hostingEnv.WebRootPath + $@"\uploads\{filename}";
                size += model.CsProject.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    model.CsProject.CopyTo(fs);
                    fs.Flush();
                }
                ViewBag.Message = $"{files.Count} file(s) / {size} bytes uploaded successfully!";
            }

            catch (Exception ex)
            {
            }

            return View();
        }

        public IActionResult ATP()
        {
            ViewData["Message"] = "Aici se vor incarca temele pentru disciplina Algoritmi si Tehnici de programare.";

            return View();
        }

        public IActionResult BPC()
        {
            ViewData["Message"] = "Aici se vor incarca temele pentru disciplina Bazele Programarii Calculatoarelor.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
