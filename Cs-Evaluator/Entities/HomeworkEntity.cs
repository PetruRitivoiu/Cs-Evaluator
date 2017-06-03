using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsEvaluator.Entities
{
    public class HomeworkEntity
    {
        public int ID { get; set; }
        public string FileName { get; set; }

        public bool WasEvaluated { get; set; } = false;
        public int EvaluationResult { get; set; } = -1;

        public virtual HomeworkDescriptionEntity HomeworkDescription { get; set; }
        public virtual StudentEntity Student { get; set; }
    }
}
