using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsEvaluator.Entities
{
    public class HomeworkDescriptionEntity
    {
        public int ID { get; set; }

        public string fullname { get; set; }

        public string shortDescription { get; set; }
        public string fullDescription { get; set; }

        public string initialFile { get; set; }
        public string expectedFile { get; set; }

        public SubjectEntity Subject { get; set; }
    }
}
