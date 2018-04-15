using CsEvaluator.Data.Entities;
using System.Collections.Generic;

namespace CsEvaluator.Repository.Interfaces
{
    public interface IHomeworkDescriptionRepository
    {
        List<HomeworkDescriptionEntity> GetAll();
    }
}
