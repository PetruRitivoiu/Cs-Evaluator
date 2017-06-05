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
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using CsEvaluator.SqlHelper;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using EFLogging;
using EvaluatorEngine;
using CsEvaluator.Entities;
using CsEvaluator.ModelState;

namespace Cs_Evaluator.Controllers
{
    public class AppController : Controller
    {
        private IHostingEnvironment _hostingEnv;
        private IConfigurationRoot _config;
        private CsEvaluatorContext _context;
        private ILogger _logger;

        //Constructor

        public AppController(IHostingEnvironment env, IConfigurationRoot config, ILoggerFactory loggerFactory, CsEvaluatorContext context)
        {
            _hostingEnv = env;
            _config = config;
            _context = context;
            _logger = loggerFactory.CreateLogger("AppController");
        }

        //Utilitary methods

        private void wrapStudentsData(HomeworkViewModel model)
        {
            List<StudentPreviewModel> studentsPreview = new List<StudentPreviewModel>();
            IEnumerable<StudentEntity> students = _context.Students.ToList();

            foreach (StudentEntity student in students)
            {
                studentsPreview.Add(new StudentPreviewModel() { fullname = student.Forename + " " + student.Surname, ID = student.ID });
            }

            model.Students = studentsPreview;

        }

        private void wrapHomeworkDescriptionData(HomeworkViewModel model)
        {
            List<HomeworkDescriptionPreviewModel> homeworkDescriptionsPreview = new List<HomeworkDescriptionPreviewModel>();
            IEnumerable<HomeworkDescriptionEntity> homeworkDescriptions = _context.HomeworkDescriptions.ToList();

            foreach (HomeworkDescriptionEntity hde in homeworkDescriptions)
            {
                homeworkDescriptionsPreview.Add(new HomeworkDescriptionPreviewModel() { ID = hde.ID, fullname = hde.fullname, fullDescription = hde.fullDescription });
            }

            model.HomeworkDescriptions = homeworkDescriptionsPreview;
        }

        private void processFileUpload(HomeworkViewModel model)
        {
            string filename = null;
            long size = 0;
            try
            {
                filename = ContentDispositionHeaderValue
                    .Parse(model.CsProject.ContentDisposition)
                    .FileName
                    .Trim('"');
                filename = $@"C:\Users\thinkpad-e560\\Documents\Visual Studio 2017\Projects\cs-evaluator\EvaluatorEngine\uploads\{filename}";
                size += model.CsProject.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    model.CsProject.CopyTo(fs);
                    fs.Flush();
                }
                ViewBag.Message = $"{size} bytes uploaded successfully!";
            }

            catch (Exception ex)
            {
                _logger.LogError("\r\nAppController -- file upload failed: \r\n" + ex.StackTrace);
            }
        }


        //Controller methods
        public IActionResult Index()
        {
            ViewData["Message"] = "CsEvaluator: Pagina principala";
            return View();
        }

        [ImportModelState]
        public IActionResult BPC(HomeworkViewModel model)
        {
            wrapStudentsData(model);

            wrapHomeworkDescriptionData(model);

            ViewData["Message"] = "Aici se vor incarca temele pentru disciplina Bazele Programarii Calculatoarelor.";

            return View(model);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult BPC(HomeworkViewModel model, IList<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                wrapStudentsData(model);

                wrapHomeworkDescriptionData(model);

                //file processing

                processFileUpload(model);

                //form processing
                HomeworkEntity he = new HomeworkEntity()
                {
                    FileName = model.CsProject.FileName,
                    HomeworkDescription = _context.HomeworkDescriptions.FirstOrDefault(t => t.ID == model.HomeworkDescriptionID),
                    Student = _context.Students.FirstOrDefault(t => t.ID == model.StudentID)
                };

                StudentEntity se = _context.Students.FirstOrDefault(t => t.ID == model.StudentID);
                se.Homeworks.Add(he);
                
                //compile and execute
                //save data in database

                _context.Homeworks.Add(he);

                _context.SaveChanges();

                
                
                return RedirectToAction("Results", new { homeworkID = he.ID, StudentID = model.StudentID});
            }

            return RedirectToAction("BPC");
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

        public IActionResult Results(int homeworkID, int studentID)
        {
            ViewData["Message"] = "Rezultatele evaluarii";

            BPC bpc = new BPC();

            HomeworkEntity he = _context.Homeworks.Include(t => t.Student)
                .Include(t => t.HomeworkDescription)
                .ThenInclude(t => t.Subject)
                .FirstOrDefault(t => t.ID == homeworkID);
            StudentEntity se = he.Student;

            ResultViewModel model = new ResultViewModel();

            model.StudentName = se.Forename + " " + se.Surname;
            model.SubjectName = he.HomeworkDescription.Subject.Name;
            model.HomeworkName = he.HomeworkDescription.fullname;
            model.HomeworkDescription = he.HomeworkDescription.fullDescription;
            model.EvaluationResult = he.EvaluationResult;

            var pathToFile = $@"uploads\{he.FileName}"; // -> Arg 1
            var exeFile = he.FileName.Substring(0, he.FileName.LastIndexOf('.')) + "_"  + "_" + he.ID + ".exe"; // -> Arg 2
            var validationFile = $@"uploads\validation_files\{he.HomeworkDescription.ID}\{he.HomeworkDescription.initialFile}"; // -> Arg 3
            var expectedFile = $@"uploads\validation_files\{he.HomeworkDescription.ID}\{he.HomeworkDescription.expectedFile}"; // -> Arg 4


            string[] args = { pathToFile, exeFile, validationFile, expectedFile};

            bpc.CompileAndScanFile(args);
            Evaluation eval = bpc.Evaluate(args);


            model.Errors = String.IsNullOrEmpty(eval.StdError) ? "None" : eval.StdError;
            model.EvaluationResult = eval.EvaluationResult;

            return View(model);
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
