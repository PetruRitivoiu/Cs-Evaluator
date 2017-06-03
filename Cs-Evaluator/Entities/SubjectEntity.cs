using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsEvaluator.Entities
{
    public class SubjectEntity
    {
        public SubjectEntity()
        {
            StudentSubjectRelationship = new HashSet<StudentSubjectRelationship>();
            HomeworkDescriptions = new HashSet<HomeworkDescriptionEntity>();
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<StudentSubjectRelationship> StudentSubjectRelationship { get; set; }
        public virtual ICollection<HomeworkDescriptionEntity> HomeworkDescriptions { get; set; }
    }
}
