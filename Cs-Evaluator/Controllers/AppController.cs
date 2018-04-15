using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CsEvaluator.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CsEvaluator.Data.Entities;
using CsEvaluator.ModelState;
using CsEvaluator.Repository.Interfaces;
using CsEvaluator.Engine;


namespace CsEvaluator.Controllers
{
    public class AppController : Controller
    {
        private IHostingEnvironment _hostingEnv;
        private IConfigurationRoot _config;
        private IAppRepository _repository;
        private IEvaluator _evaluator;
        private ILogger _logger;

        public AppController(IHostingEnvironment env, IConfigurationRoot config, ILoggerFactory loggerFactory, IAppRepository repository, IEvaluator evaluator)
        {
            _hostingEnv = env;
            _config = config;
            _repository = repository;
            _evaluator = evaluator;
            _logger = loggerFactory.CreateLogger("AppController");
        }


        //Utilitary methods
        private HomeworkViewModel WrapStudentsData(HomeworkViewModel model)
        {
            List<StudentPreviewModel> studentsPreview = new List<StudentPreviewModel>();
            IEnumerable<StudentEntity> students = _repository.StudentRepository.GetAll();

            foreach (StudentEntity student in students)
            {
                studentsPreview.Add(new StudentPreviewModel() { Fullname = student.Forename + " " + student.Surname, ID = student.ID });
            }

            model.Students = studentsPreview;

            return model;
        }

        private HomeworkViewModel WrapHomeworkDescriptionData(HomeworkViewModel model)
        {
            List<HomeworkDescriptionPreviewModel> homeworkDescriptionsPreview = new List<HomeworkDescriptionPreviewModel>();
            IEnumerable<HomeworkDescriptionEntity> homeworkDescriptions = _repository.HomeworkDescriptionRepository.GetAll();

            foreach (HomeworkDescriptionEntity hde in homeworkDescriptions)
            {
                homeworkDescriptionsPreview.Add(new HomeworkDescriptionPreviewModel() { ID = hde.ID, Fullname = hde.Name, FullDescription = hde.FullDescription });
            }

            model.HomeworkDescriptions = homeworkDescriptionsPreview;

            return model;
        }

        private HomeworkViewModel WrapValidationFiles(HomeworkViewModel model)
        {
            //model.ReflectionValidationFile = _repository.

            return null;
        }

        private HomeworkViewModel ProcessFileUpload(HomeworkViewModel model)
        {
            string filename = null;
            try
            {

                filename = ContentDispositionHeaderValue
                    .Parse(model.CsProject.ContentDisposition)
                    .FileName
                    .ToString();

                filename = $@"C:\Users\thinkpad-e560\\Documents\Visual Studio 2017\Projects\cs-evaluator\CsEvaluator.Engine\uploads\{model.CsProject.FileName}";
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    model.CsProject.CopyTo(fs);
                    fs.Flush();
                }
            }

            catch (Exception ex)
            {
                _logger.LogError("\r\nAppController -- file upload failed: \r\n" + ex.StackTrace);
                throw;
            }

            return model;
        }


        //Controller methods
        public IActionResult Index()
        {
            ViewData["Message"] = "CsEvaluator: Pagina principala";
            return View();
        }


        [ImportModelState]
        public IActionResult PAW(HomeworkViewModel model)
        {
            model = WrapStudentsData(model);

            model = WrapHomeworkDescriptionData(model);

            ViewData["Message"] = "Aici se vor incarca temele pentru disciplina Programarea Aplicatiilor Windows.";

            return View(model);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult PAW(HomeworkViewModel model, IList<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                model = WrapStudentsData(model);

                model = WrapHomeworkDescriptionData(model);

                model = ProcessFileUpload(model);

                //compile and execute and then save data to DB
                //Evaluation eval = _evaluator.Evaluate(model.CsProject.FileName, /*trebuie adus in model de la inceput*/);

                model.WasEvaluated = false;
                model.EvaluationResult = -1;

                //returns HomeworkID
                var homeworkID = _repository.HomeworkRepository.Add(model);

                return RedirectToAction("Results", new { HomeworkID = homeworkID, model.StudentID });
            }

            return RedirectToAction("PAW");
        }


        [ImportModelState]
        public IActionResult PAWAdmin()
        {
            return View();
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult PAWAdmin(HomeworkDescriptionViewModel model, IList<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                //do something
            }

            return RedirectToAction("PAWAdmin");
        }


        public IActionResult Subjects()
        {
            var data = _repository.SubjectsRepository.GetAll();
            var model = new SubjectsViewModel
            {
                Subjects = data
            };
            return View(model);
        }

        public IActionResult Results(int homeworkID, int studentID)
        {
            ViewData["Message"] = "Rezultatele evaluarii";

            HomeworkEntity he = _repository.HomeworkRepository.GetAll()
                .FirstOrDefault(t => t.ID == homeworkID);

            ResultViewModel model = new ResultViewModel
            {
                StudentName = he.Student.Forename + " " + he.Student.Surname,
                SubjectName = he.HomeworkDescription.Subject.Name,
                HomeworkName = he.HomeworkDescription.Name,
                HomeworkDescription = he.HomeworkDescription.FullDescription,
                EvaluationResult = he.EvaluationResult
            };

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
