using Newtonsoft.Json;

namespace CsEvaluator.Engine.Common
{
    public class Evaluation
    {
        [JsonConstructor]
        private Evaluation() { }

        public Evaluation (StaticEvaluation staticEval, FunctionalEvaluation functionalEval)
        {
            StaticEvaluation = staticEval;
            FunctionalEvaluation = functionalEval;
        }

        public Evaluation(string error)
        {
            Error = error;
        }

        public StaticEvaluation StaticEvaluation { get; set; }

        public FunctionalEvaluation FunctionalEvaluation { get; set; }

        public double EvaluationResult
        {
            get
            {
                if (StaticEvaluation == null || FunctionalEvaluation == null)
                {
                    return -1;
                }

                return (StaticEvaluation.EvaluationResult + FunctionalEvaluation.EvaluationResult) / 2;
            }
        }

        public string Error { get; set; }
    }
}
