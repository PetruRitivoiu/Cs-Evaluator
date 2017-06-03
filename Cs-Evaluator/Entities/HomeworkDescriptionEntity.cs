using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsEvaluator.Entities
{
    public class HomeworkDescriptionEntity
    {
        public HomeworkDescriptionEntity()
        {
            Homeworks = new HashSet<HomeworkEntity>();
        }

        public int ID { get; set; }

        public string fullname { get; set; }

        public string shortDescription { get; set; }
        public string fullDescription { get; set; }

        public string initialFile { get; set; } = "initial.txt";
        public string expectedFile { get; set; } = "expected.txt";

        public virtual SubjectEntity Subject { get; set; }
        public virtual ICollection<HomeworkEntity> Homeworks { get; set; }
    }
}
