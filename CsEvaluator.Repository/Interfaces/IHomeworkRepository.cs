using CsEvaluator.Data.Entities;
using CsEvaluator.Data.ViewModels;
using System.Collections.Generic;

namespace CsEvaluator.Repository.Interfaces
{
    public interface IHomeworkRepository
    {
        int Add(HomeworkViewModel hvm);

        List<HomeworkEntity> GetAll();
    }
}
