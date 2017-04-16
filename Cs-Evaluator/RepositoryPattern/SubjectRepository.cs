using CsEvaluator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CsEvaluator.SqlHelper;

namespace CsEvaluator.RepositoryPattern
{
    public class SubjectRepository : Repository<SubjectEntity>, ISubjectRepository
    {
        public SubjectRepository(CsEvaluatorContext context) : base(context)
        {
        }
    }
}
