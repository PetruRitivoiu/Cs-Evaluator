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
using CsEvaluator.Entities;
using CsEvaluator.ModelState;

namespace Cs_Evaluator.Controllers
{
    public class AppController : Controller
    {
        private IHostingEnvironment _hostingEnv;
        private IConfigurationRoot _config;
        private CsEvaluatorContext _context;
        private IUnitOfWork _unitOfWork;
        private ILogger _logger;

        //Constructor

        public AppController(IHostingEnvironment env, IConfigurationRoot config, IUnitOfWork uof, ILoggerFactory loggerFactory, CsEvaluatorContext context)
        {
            _unitOfWork = uof;
            _hostingEnv = env;
            _config = config;
            _context = context;
            _logger = loggerFactory.CreateLogger("AppController");
        }

        //Utilitary methods

        private void wrapStudentsData(HomeworkViewModel model)
        {
            List<StudentPreviewModel> studentsPreview = new List<StudentPreviewModel>();
            IEnumerable<StudentEntity> students = _unitOfWork.StudentRepository.GetAll();

            foreach (StudentEntity student in students)
            {
                studentsPreview.Add(new StudentPreviewModel() { fullname = student.Forename + " " + student.Surname, ID = student.ID });
            }

            model.Students = studentsPreview;

        }

        private void wrapHomeworkDescriptionData(HomeworkViewModel model)
        {
            List<HomeworkDescriptionPreview> homeworkDescriptionsPreview = new List<HomeworkDescriptionPreview>();
            IEnumerable<HomeworkDescriptionEntity> homeworkDescriptions = _unitOfWork.HomeworkDescriptionRepository.GetAll();

            foreach (HomeworkDescriptionEntity hde in homeworkDescriptions)
            {
                homeworkDescriptionsPreview.Add(new HomeworkDescriptionPreview() { ID = hde.ID, fullname = hde.fullname });
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
                filename = _hostingEnv.WebRootPath + $@"\uploads\{filename}";
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
                    Subject = _unitOfWork.SubjectRepository.Find(t => t.Name.Equals("BPC")).FirstOrDefault(),
                    HomeworkDescription = _unitOfWork.HomeworkDescriptionRepository.Find(t => t.ID == model.HomeworkDescriptionID).FirstOrDefault(),
                };

                StudentHomeworkRelationship shr = new StudentHomeworkRelationship()
                {
                    Homework = he,
                    Student = _unitOfWork.StudentRepository.Find(t => t.ID == model.StudentID).FirstOrDefault()
                };

                //compile and execute
                //save data in database

                _unitOfWork.HomeworkRepository.Add(he);
                _unitOfWork.StudentHomeworkRepository.Add(shr);

                _unitOfWork.Complete();

                
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
            ViewData["Message"] = "Rezultatele evaluarilor de pana in prezent ** FIX REPOSITORY PATTERN ISSUE **";

            ResultViewModel model = new ResultViewModel();

            HomeworkEntity he = _unitOfWork.HomeworkRepository.Get(homeworkID);
            StudentHomeworkRelationship shr = _unitOfWork.StudentHomeworkRepository.Find(t => t.HomeworkID == homeworkID && t.StudentID == studentID).FirstOrDefault();
            StudentEntity se = _unitOfWork.StudentRepository.Get(studentID);

            model.StudentName = se.Forename + " " + se.Surname;
            model.SubjectName = "fix this";
            model.HomeworkName = "fix this";
            model.HomeworkDescription = "fix this";
            model.EvaluationResult = he.EvaluationResult;

            //TBD
            BPC bpc = new BPC();
            //string str = bpc.Evaluate(" \"C:\\Users\\thinkpad-e560\\Documents\\Visual Studio 2017\\Projects\\cs-evaluator\\Cs-Evaluator\\wwwroot\\uploads\\homework_test.cs\", hello world!");
            string str = bpc.Evaluate(" \"C:\\Users\\thinkpad-e560\\Documents\\Visual Studio 2017\\Projects\\cs-evaluator\\Cs-Evaluator\\wwwroot\\uploads\\" + he.FileName + "\"" + " , hello world!");

            model.rawViewForTest = str;

            return View(model);
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
