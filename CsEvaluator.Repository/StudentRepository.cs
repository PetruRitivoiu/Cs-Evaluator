using System.Collections.Generic;
using System.Linq;
using CsEvaluator.Data.Entities;
using CsEvaluator.Database.SqlHelper;

namespace CsEvaluator.Repository.Interfaces
{
    public class StudentRepository : IStudentRepository
    {
        private CsEvaluatorContext Context { get; }

        public StudentRepository(CsEvaluatorContext context)
        {
            Context = context;
        }

        public List<StudentEntity> GetAll()
        {
            return Context.Students.ToList();
        }
    }
}
