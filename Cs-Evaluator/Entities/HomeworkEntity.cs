using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cs_Evaluator.Entities
{
    public class HomeworkEntity
    {
        public int ID { get; set; }
        public string FullPath { get; set; }

        public SubjectEntity Subject { get; set; }
        public ICollection<StudentEntity> Students { get; set; }

    }
}
