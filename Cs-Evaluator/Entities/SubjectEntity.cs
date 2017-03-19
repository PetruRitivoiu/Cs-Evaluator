using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsEvaluator.Entities
{
    public class SubjectEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<StudentSubjectRelationship> StudentSubjectRelationship { get; set; }
        public ICollection<HomeworkEntity> Homeworks { get; set; }
    }
}
