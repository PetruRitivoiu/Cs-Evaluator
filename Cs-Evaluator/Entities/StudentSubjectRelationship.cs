using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsEvaluator.Entities
{
    public class StudentSubjectRelationship
    {
        public int StudentID { get; set; }
        public virtual StudentEntity Student { get; set; }

        public int SubjectID { get; set; }
        public virtual SubjectEntity Subject { get; set; }
    }
}
