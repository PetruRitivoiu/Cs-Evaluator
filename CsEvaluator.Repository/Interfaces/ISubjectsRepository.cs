using System;
using System.Collections.Generic;
using CsEvaluator.Data.Entities;

namespace CsEvaluator.Repository.Interfaces
{
    public interface ISubjectsRepository
    {
        List<SubjectEntity> GetAll();
    }
}
