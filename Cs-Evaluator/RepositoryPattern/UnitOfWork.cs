using CsEvaluator.SqlHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsEvaluator.RepositoryPattern
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CsEvaluatorContext Context;

        public UnitOfWork(CsEvaluatorContext context)
        {
            this.Context = context;

            StudentRepository = new StudentRepository(Context);
            SubjectRepository = new SubjectRepository(Context);
            HomeworkRepository = new HomeworkRepository(Context);
        }

        public IStudentRepository StudentRepository { get; private set; }

        public ISubjectRepository SubjectRepository { get; private set; }

        public IHomeworkRepository HomeworkRepository { get; private set; }

        public int Complete()
        {
            return Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
