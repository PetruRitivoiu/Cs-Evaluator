using CsEvaluator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CsEvaluator.RepositoryPattern
{
    public class HomeworkDescriptionRepository : Repository<HomeworkDescriptionEntity>, IHomeworkDescriptionRepository
    {
        public HomeworkDescriptionRepository(DbContext context) : base(context)
        {
        }
    }
}
