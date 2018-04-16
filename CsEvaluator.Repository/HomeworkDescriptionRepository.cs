using CsEvaluator.Data.Entities;
using CsEvaluator.Data.ViewModels;
using CsEvaluator.Database.SqlHelper;
using CsEvaluator.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace CsEvaluator.Repository
{
    public class HomeworkDescriptionRepository : IHomeworkDescriptionRepository
    {
        private CsEvaluatorContext Context { get; }

        public HomeworkDescriptionRepository(CsEvaluatorContext context)
        {
            Context = context;
        }


        public List<HomeworkDescriptionEntity> GetAll()
        {
            return Context.HomeworkDescriptions.ToList();
        }

        public int Add(HomeworkDescriptionViewModel hdvm)
        {
            var hde = new HomeworkDescriptionEntity
            {
                Name = hdvm.Name,
                ShortDescription = hdvm.ShortDescription,
                FullDescription = hdvm.FullDescription,
                ReflectionFile = hdvm.ReflectionFile.FileName,
                UnitTestsFile = hdvm.UnitTestFile.FileName
            };

            var subject = Context.Subjects.FirstOrDefault(s => s.Name == hdvm.Subject);
            hde.Subject = subject;

            Context.HomeworkDescriptions.Add(hde);
            Context.SaveChanges();

            return hde.ID;
        }
    }
}
