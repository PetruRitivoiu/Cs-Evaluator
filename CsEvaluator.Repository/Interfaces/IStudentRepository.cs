using System;
using System.Collections.Generic;
using CsEvaluator.Data.Entities;

namespace CsEvaluator.Repository.Interfaces
{
    public interface IStudentRepository
    {
        List<StudentEntity> GetAll();
    }
}
