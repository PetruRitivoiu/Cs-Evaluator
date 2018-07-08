using CsEvaluator.Engine.ReflectionEvaluator.Rules;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CsEvaluator.Engine.Common
{
    public class StaticEvaluation
    {
        [JsonConstructor]
        public StaticEvaluation() { }

        public StaticEvaluation(double evaluationResult, List<RuleEvaluation> rulesEvaluation)
        {
            EvaluationResult = evaluationResult;
            RulesEvaluation = rulesEvaluation;
        }

        public double EvaluationResult { get; set; }

        public List<RuleEvaluation> RulesEvaluation { get; set; }
    }
}
