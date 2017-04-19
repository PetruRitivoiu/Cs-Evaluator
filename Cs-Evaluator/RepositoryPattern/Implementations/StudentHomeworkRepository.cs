using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CsEvaluator.Entities;
using Microsoft.EntityFrameworkCore;

namespace CsEvaluator.RepositoryPattern
{
    public class StudentHomeworkRepository : Repository<StudentHomeworkRelationship>, IStudentHomeworkRepository
    {
        public StudentHomeworkRepository(DbContext context) : base(context)
        {
        }

    }
}
