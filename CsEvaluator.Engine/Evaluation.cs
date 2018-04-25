using CsEvaluator.Engine.ReflectionEvaluator.Rules;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CsEvaluator.Engine
{
    public class Evaluation
    {
        [JsonConstructor]
        private Evaluation() { }

        public Evaluation(double evaluationResult, List<RuleEvaluation> rulesEvaluation)
        {
            EvaluationResult = evaluationResult;
            RulesEvaluation = rulesEvaluation;
        }

        public Evaluation(string error)
        {
            EvaluationResult = -1;
            Error = error;
        }

        public double EvaluationResult { get; set; }

        public List<RuleEvaluation> RulesEvaluation{ get; set; }

        public string Error { get; set; }
    }
}
