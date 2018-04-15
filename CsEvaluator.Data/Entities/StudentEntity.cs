using System.Collections.Generic;


namespace CsEvaluator.Data.Entities
{
    public class StudentEntity
    {
        public StudentEntity()
        {
            Homeworks = new HashSet<HomeworkEntity>();
            StudentSubjectRelationship = new HashSet<StudentSubjectRelationship>();
        }

        public int ID { get; set; }
        public string Surname { get; set; }
        public string Forename { get; set; }

        public virtual ICollection<HomeworkEntity> Homeworks { get; set; }
        public virtual ICollection<StudentSubjectRelationship> StudentSubjectRelationship { get; set; }
    }
}
