using CsEvaluator.Data.Entities;
using CsEvaluator.Data.ViewModels;
using CsEvaluator.Database.SqlHelper;
using CsEvaluator.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;


namespace CsEvaluator.Repository
{
    public class HomeworkRepository : IHomeworkRepository
    {
        private CsEvaluatorContext Context { get; }

        public HomeworkRepository(CsEvaluatorContext context)
        {
            Context = context;
        }

        public int Add(HomeworkViewModel model)
        {
            HomeworkEntity he = new HomeworkEntity()
            {
                FileName = model.CsProject.FileName,
                HomeworkDescription = Context.HomeworkDescriptions.FirstOrDefault(t => t.ID == model.HomeworkDescriptionID),
                Student = Context.Students.FirstOrDefault(t => t.ID == model.StudentID),
                EvaluationResult = model.EvaluationResult
            };

            Context.Homeworks.Add(he);

            Context.SaveChanges();

            return he.ID;
        }

        public List<HomeworkEntity> GetAll()
        {
            return Context.Homeworks.Include(t => t.Student)
                .Include(t => t.HomeworkDescription)
                .ThenInclude(t => t.Subject).ToList();
        }
    }
}
