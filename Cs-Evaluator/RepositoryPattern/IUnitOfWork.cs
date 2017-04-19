using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsEvaluator.RepositoryPattern
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository StudentRepository { get; }
        ISubjectRepository SubjectRepository { get; }
        IHomeworkRepository HomeworkRepository { get; }
        IHomeworkDescriptionRepository HomeworkDescriptionRepository { get; }
        IStudentHomeworkRepository StudentHomeworkRepository { get; }

        int Complete();
    }
}
