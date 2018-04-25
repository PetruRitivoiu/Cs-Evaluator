using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsEvaluator.Data.Entities
{
    public class HomeworkEntity
    {
        public int ID { get; set; }
        public string FileName { get; set; }

        public double EvaluationResult { get; set; } = -1;

        public virtual HomeworkDescriptionEntity HomeworkDescription { get; set; }
        public virtual StudentEntity Student { get; set; }
    }
}
