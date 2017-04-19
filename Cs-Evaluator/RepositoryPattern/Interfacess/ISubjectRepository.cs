using CsEvaluator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsEvaluator.RepositoryPattern
{
    public interface ISubjectRepository : IRepository<SubjectEntity>
    {
    }
}
