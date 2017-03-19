using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsEvaluator.Entities
{
    public class StudentEntity
    {
        public int ID { get; set; }
        public string Surname { get; set; }
        public string Forename { get; set; }

        public ICollection<StudentHomeworkRelationship> StudentHomeworkRelationship { get; set; }
        public ICollection<StudentSubjectRelationship> StudentSubjectRelationship { get; set; }
    }
}
