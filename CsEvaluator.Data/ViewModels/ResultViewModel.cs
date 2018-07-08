using CsEvaluator.Engine.Common;

namespace CsEvaluator.Data.ViewModels
{
    public class ResultViewModel
    {
        public string SubjectName { get; set; }

        public string HomeworkName { get; set; }

        public string HomeworkDescription { get; set; }

        public string StudentName { get; set; }

        public Evaluation Evaluation { get; set; }
    }
}
