using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsEvaluator.Entities
{
    public class HomeworkEntity
    {
        public int ID { get; set; }
        public string FullPath { get; set; }

        public SubjectEntity Subject { get; set; }
        public ICollection<StudentHomeworkRelationship> StudentHomeworkRelationship { get; set; }

        public HomeworkDescriptionEntity HomeworkDescription { get; set; }
    }
}
