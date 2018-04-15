using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsEvaluator.Data.Entities
{
    public class HomeworkDescriptionEntity
    {
        public HomeworkDescriptionEntity()
        {
            Homeworks = new HashSet<HomeworkEntity>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }

        public string ReflectionFile { get; set; }
        public string UnitTestsFile { get; set; }

        public virtual SubjectEntity Subject { get; set; }
        public virtual ICollection<HomeworkEntity> Homeworks { get; set; }
    }
}
