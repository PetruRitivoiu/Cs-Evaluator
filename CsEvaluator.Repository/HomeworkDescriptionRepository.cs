using CsEvaluator.Data.Entities;
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
    }
}
