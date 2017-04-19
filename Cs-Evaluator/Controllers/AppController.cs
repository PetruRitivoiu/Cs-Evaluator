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

namespace Cs_Evaluator.Controllers
{
    public class AppController : Controller
    {
        private IHostingEnvironment _hostingEnv;
        private IConfigurationRoot _config;
        private CsEvaluatorContext _context;
        private IUnitOfWork _unitOfWork;
        private ILogger _logger;

        public AppController(IHostingEnvironment env, IConfigurationRoot config, IUnitOfWork uof, ILoggerFactory loggerFactory, CsEvaluatorContext context)
        {
            _unitOfWork = uof;
            _hostingEnv = env;
            _config = config;
            _context = context;
            _logger = loggerFactory.CreateLogger("AppController");

            //TBD
            //BPC bpc = new BPC();
            //string str = bpc.Evaluate(" \"C:\\Users\\thinkpad-e560\\Documents\\Visual Studio 2017\\Projects\\cs-evaluator\\Cs-Evaluator\\wwwroot\\uploads\\homework_test.cs\", hello world!");
        }

        public IActionResult Index()
        {
            ViewData["Message"] = "CsEvaluator: Pagina principala";
            return View();
        }

        public IActionResult BPC(HomeworkViewModel model)
        {
            List<StudentPreviewModel> studentsPreview = new List<StudentPreviewModel>();
            IEnumerable<StudentEntity> students = _unitOfWork.StudentRepository.GetAll();

            foreach (StudentEntity student in students)
            {
                studentsPreview.Add(new StudentPreviewModel() { fullname = student.Forename + " " + student.Surname, ID = student.ID });
            }

            model.Students = studentsPreview;

            //---

            List<HomeworkDescriptionPreview> homeworkDescriptionsPreview = new List<HomeworkDescriptionPreview>();
            IEnumerable<HomeworkDescriptionEntity> homeworkDescriptions = _unitOfWork.HomeworkDescriptionRepository.GetAll();

            foreach (HomeworkDescriptionEntity hde in homeworkDescriptions)
            {
                homeworkDescriptionsPreview.Add(new HomeworkDescriptionPreview() { ID = hde.ID, fullname = hde.fullname });
            }

            model.HomeworkDescriptions = homeworkDescriptionsPreview;

            //---

            ViewData["Message"] = "Aici se vor incarca temele pentru disciplina Bazele Programarii Calculatoarelor.";

            return View(model);
        }

        [HttpPost]
        public IActionResult BPC(HomeworkViewModel model, IList<IFormFile> files)
        {
            List<StudentPreviewModel> studentsPreview = new List<StudentPreviewModel>();
            IEnumerable<StudentEntity> students = _unitOfWork.StudentRepository.GetAll();

            foreach (StudentEntity student in students)
            {
                studentsPreview.Add(new StudentPreviewModel() { fullname = student.Forename + " " + student.Surname, ID = student.ID });
            }

            model.Students = studentsPreview;

            //---

            List<HomeworkDescriptionPreview> homeworkDescriptionsPreview = new List<HomeworkDescriptionPreview>();
            IEnumerable<HomeworkDescriptionEntity> homeworkDescriptions = _unitOfWork.HomeworkDescriptionRepository.GetAll();

            foreach (HomeworkDescriptionEntity hde in homeworkDescriptions)
            {
                homeworkDescriptionsPreview.Add(new HomeworkDescriptionPreview() { ID = hde.ID, fullname = hde.fullname });
            }

            model.HomeworkDescriptions = homeworkDescriptionsPreview;

            //---

            //file processing
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


            //form processing
            HomeworkEntity he = new HomeworkEntity()
            {
                FullPath = filename,
                Subject = _unitOfWork.SubjectRepository.Find(t => t.Name.Equals("BPC")).FirstOrDefault(),
                HomeworkDescription = _unitOfWork.HomeworkDescriptionRepository.Find(t => t.ID == model.HomeworkDescriptionID).FirstOrDefault(),
            };

            StudentHomeworkRelationship shr = new StudentHomeworkRelationship()
            {
                Homework = he,
                Student = _unitOfWork.StudentRepository.Find(t => t.ID == model.StudentID).FirstOrDefault()
            };

            _unitOfWork.HomeworkRepository.Add(he);
            _unitOfWork.StudentHomeworkRepository.Add(shr);

            _unitOfWork.Complete();

            return View(model);
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


        public IActionResult Error()
        {
            return View();
        }
    }
}
