using CsEvaluator.Database.SqlHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsEvaluator.Repository.Interfaces
{
    public class AppRepository : IAppRepository
    {
        public AppRepository(CsEvaluatorContext context)
        {
            HomeworkDescriptionRepository = new HomeworkDescriptionRepository(context);
            HomeworkRepository = new HomeworkRepository(context);
            SubjectsRepository = new SubjectsRepository(context);
            StudentRepository = new StudentRepository(context);
        }

        public IHomeworkDescriptionRepository HomeworkDescriptionRepository { get;set;}

        public IHomeworkRepository HomeworkRepository { get; set; }

        public ISubjectsRepository SubjectsRepository { get; set; }

        public IStudentRepository StudentRepository { get ; set; }
    }
}
