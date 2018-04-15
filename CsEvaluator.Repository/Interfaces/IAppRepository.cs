using System;
using System.Collections.Generic;
using System.Text;

namespace CsEvaluator.Repository.Interfaces
{
    public interface IAppRepository
    {
        IHomeworkDescriptionRepository HomeworkDescriptionRepository { get; set; }

        IHomeworkRepository HomeworkRepository {get; set; }

        ISubjectsRepository SubjectsRepository { get; set; }

        IStudentRepository StudentRepository { get; set; }
    }
}
