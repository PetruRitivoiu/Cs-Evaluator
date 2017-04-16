using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CsEvaluator.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using CsEvaluator.SqlHelper;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using EFLogging;
using EvaluatorEngine;
using CsEvaluator.RepositoryPattern;

namespace Cs_Evaluator.Controllers
{
    public class AppController : Controller
    {
        private IHostingEnvironment _hostingEnv;
        private IConfigurationRoot _config;
        private CsEvaluatorContext _context;
        private IUnitOfWork _unitOfWork;

        public AppController(IHostingEnvironment env, IConfigurationRoot config, IUnitOfWork uof , CsEvaluatorContext context)
        {
            _unitOfWork = uof;
            _hostingEnv = env;
            _config = config;
            _context = context;

            //TBD
            BPC bpc = new BPC();
            string str = bpc.Evaluate(" \"C:\\Users\\thinkpad-e560\\Documents\\Visual Studio 2017\\Projects\\cs-evaluator\\Cs-Evaluator\\wwwroot\\uploads\\homework_test.cs\", hello world!");
        }

        public IActionResult Index()
        {
            ViewData["Message"] = "CsEvaluator: Pagina principala";
            return View();
        }

        [HttpPost]
        public IActionResult BPC(HomeworkViewModel model, IList<IFormFile> files)
        {

            long size = 0;
            try
            {
                var filename = ContentDispositionHeaderValue
                    .Parse(model.CsProject.ContentDisposition)
                    .FileName
                    .Trim('"');
                filename = _hostingEnv.WebRootPath + $@"\uploads\{filename}";
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
                //do something
            }

            return View();
        }

        public IActionResult Subjects()
        {
            var data = _context.Subjects.ToList();
            var model = new SubjectsViewModel();
            model.Subjects = data;
            return View(model);
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
