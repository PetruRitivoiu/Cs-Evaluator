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
using CsEvaluator.Engine.Common;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO.Compression;

namespace CsEvaluator.Controllers.Web
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

        private async Task<string> ProcessFileUpload(IFormFile file, string basePath)
        {
            string filename = null;
            try
            {

                filename = ContentDispositionHeaderValue
                    .Parse(file.ContentDisposition)
                    .FileName
                    .Value;

                filename = Path.Combine(basePath, file.FileName);
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    await file.CopyToAsync(fs);
                    fs.Flush();
                }

                return filename;
            }

            catch (Exception ex)
            {
                _logger.LogError("\r\nAppController -- file upload failed: \r\n" + ex.StackTrace);
                throw;
            }
        }

        private async Task<bool> ProcessZipUpload(IFormFile file, string basePath)
        {
            string zipFile = await ProcessFileUpload(file, basePath);

            ZipFile.ExtractToDirectory(zipFile, basePath, true);

            foreach (var dir in Directory.GetDirectories(basePath))
            {
                CopyFilesFromDir(dir, basePath, true);
            }

            return true;
        }

        private void CopyFilesFromDir(string sourceDir, string targetDir, bool overwrite)
        {
            Directory.CreateDirectory(targetDir);

            foreach (var file in Directory.GetFiles(sourceDir))
                System.IO.File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)), true);

            foreach (var directory in Directory.GetDirectories(sourceDir))
                CopyFilesFromDir(directory, Path.Combine(targetDir, Path.GetFileName(directory)), true);
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

            ViewData["Message"] = "Aici se vor incarca temele pentru disciplina Programarea Aplicatiilor Windows (PAW)";

            ViewData["Details"] = "Se va incarca arhiva .zip cu proiectul dvs. C#. Arhiva trebuie sa contina fisierul .sln " +
                "in primul rand de copii al arborelui de fisiere si sa contina folderul packages in cazul in care" +
                "ati folosit nuget-uri third-party.";

            return View(model);
        }

        [HttpPost]
        [ExportModelState]
        public async Task<IActionResult> PAW(HomeworkViewModel model, IList<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                model = WrapStudentsData(model);
                model = WrapHomeworkDescriptionData(model);

                //get homework info
                var homeworkDescriptionName = _repository.HomeworkDescriptionRepository.GetById(model.HomeworkDescriptionID).Name;
                var reflectionValidationFileName = _repository.HomeworkDescriptionRepository.GetById(model.HomeworkDescriptionID).ReflectionFile;
                var unitTestingFileName = _repository.HomeworkDescriptionRepository.GetById(model.HomeworkDescriptionID).UnitTestsFile;

                //student's homework folder
                var studentsHomeworkFolder = Path.Combine(Config.BasePathToCodeFiles, model.GetHomeworkDirectory());
                var teachersHomeworkFolder = Path.Combine(Config.BasePathToValidationFiles, homeworkDescriptionName);

                //get validation files
                var reflectionFile = Path.Combine(Config.BasePathToValidationFiles, homeworkDescriptionName, reflectionValidationFileName);
                var unitTestingFile = Path.Combine(Config.BasePathToValidationFiles, homeworkDescriptionName, unitTestingFileName);

                try
                {
                    HomeworkHelper.InitFolder(studentsHomeworkFolder, teachersHomeworkFolder);
                    await ProcessZipUpload(model.CsProject, studentsHomeworkFolder);

                    //compile and execute and then save data to DB
                    Evaluation eval = await EvaluatorTaskFactory.CreateAndStart(studentsHomeworkFolder, model.CsProject.FileName, reflectionFile, unitTestingFile);

                    model.StaticEvaluationResult = eval.StaticEvaluation.EvaluationResult;
                    //TO-DO functionalEvaluationResult !!
                    TempData["evaluation"] = JsonConvert.SerializeObject(eval);

                    //returns HomeworkID
                    var homeworkID = _repository.HomeworkRepository.Add(model);

                    return RedirectToAction("Results", new { HomeworkID = homeworkID, model.StudentID });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error");
                }

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
        public async Task<IActionResult> PAWAdmin(HomeworkDescriptionViewModel model, IList<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                model.Subject = "PAW";

                var fullValidationFolder = Path.Combine(Config.BasePathToValidationFiles, model.Name);

                Directory.CreateDirectory(fullValidationFolder);

                try
                {
                    await ProcessFileUpload(model.ReflectionFile, fullValidationFolder);
                    await ProcessFileUpload(model.UnitTestFile, fullValidationFolder);
                    await ProcessFileUpload(model.NunitDllFile, fullValidationFolder);
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
