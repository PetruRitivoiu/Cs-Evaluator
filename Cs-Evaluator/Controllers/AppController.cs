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
using CsEvaluator.Engine.Util;
using Newtonsoft.Json;

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

        private async void ProcessFileUpload(IFormFile file, string basePath)
        {
            string filename = null;
            try
            {

                filename = ContentDispositionHeaderValue
                    .Parse(file.ContentDisposition)
                    .FileName
                    .ToString();

                filename = Path.Combine(basePath, file.FileName);
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    await file.CopyToAsync(fs);
                    fs.Flush();
                }
            }

            catch (Exception ex)
            {
                _logger.LogError("\r\nAppController -- file upload failed: \r\n" + ex.StackTrace);
                throw;
            }
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

                try
                {
                    ProcessFileUpload(model.CsProject, Config.BasePathToCodeFiles);
                }
                catch (Exception)
                {
                    return RedirectToAction("Error");
                }


                //get validation files
                var homeworkDescriptionName = _repository.HomeworkDescriptionRepository.GetById(model.HomeworkDescriptionID).Name;
                var reflectionValidationFile = _repository.HomeworkDescriptionRepository.GetById(model.HomeworkDescriptionID).ReflectionFile;

                var reflectionFolder = Path.Combine(Config.BasePathToValidationFiles, homeworkDescriptionName);
                var reflectionFile = Path.Combine(reflectionFolder, reflectionValidationFile);

                //compile and execute and then save data to DB
                Evaluation eval = TaskFactory.CreateAndStart(model.CsProject.FileName, reflectionFile).Result;

                model.EvaluationResult = eval.EvaluationResult;
                TempData["evaluation"] = JsonConvert.SerializeObject(eval);

                //returns HomeworkID
                var homeworkID = _repository.HomeworkRepository.Add(model);

                return RedirectToAction("Results", new { HomeworkID = homeworkID, model.StudentID });
            }

            return RedirectToAction("PAW");
        }

        [ImportModelState]
        public IActionResult PAWAdmin()
        {
            ViewData["Message"] = "Upload new Homework Definition";

            return View();
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult PAWAdmin(HomeworkDescriptionViewModel model, IList<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                model.Subject = "PAW";

                var fullValidationFolder = Path.Combine(Config.BasePathToValidationFiles, model.Name);

                Directory.CreateDirectory(fullValidationFolder);

                try
                {
                    ProcessFileUpload(model.ReflectionFile, fullValidationFolder);
                    ProcessFileUpload(model.UnitTestFile, fullValidationFolder);
                }
                catch (Exception)
                {
                    return RedirectToAction("Error");
                }

                _repository.HomeworkDescriptionRepository.Add(model);
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
                Evaluation = JsonConvert.DeserializeObject<Evaluation>(TempData["evaluation"].ToString())
            };

            TempData.Remove("evaluation");

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
