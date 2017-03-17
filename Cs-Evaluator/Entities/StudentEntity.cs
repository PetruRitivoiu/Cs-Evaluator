using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cs_Evaluator.Entities
{
    public class StudentEntity
    {
        public int ID { get; set; }
        public string Surname { get; set; }
        public string Forename { get; set; }

        public ICollection<HomeworkEntity> Homeworks { get; set; }
    }
}
