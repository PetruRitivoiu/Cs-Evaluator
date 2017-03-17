using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cs_Evaluator.Entities
{
    public class SubjectEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public HomeworkEntity Homeworks { get; set; }
    }
}
