using System.Collections.Generic;
using System.Linq;
using CsEvaluator.Data.Entities;
using CsEvaluator.Database.SqlHelper;
using CsEvaluator.Repository.Interfaces;

namespace CsEvaluator.Repository
{
    public class SubjectsRepository : ISubjectsRepository
    {
        private CsEvaluatorContext Context { get; }

        public SubjectsRepository(CsEvaluatorContext context)
        {
            Context = context;
        }

        public List<SubjectEntity> GetAll()
        {
            return Context.Subjects.ToList();
        }
    }
}
