using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsEvaluator.Entities
{
    public class StudentHomeworkRelationship
    {
        public int StudentID { get; set; }
        public StudentEntity Student { get; set; }

        public int HomeworkID { get; set; }
        public HomeworkEntity Homework { get; set; }
    }
}
