using CsEvaluator.Data.Entities;
using CsEvaluator.Data.ViewModels;
using System.Collections.Generic;

namespace CsEvaluator.Repository.Interfaces
{
    public interface IHomeworkDescriptionRepository
    {
        List<HomeworkDescriptionEntity> GetAll();

        int Add(HomeworkDescriptionViewModel hdvm);
    }
}
