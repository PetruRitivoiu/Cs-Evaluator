using Newtonsoft.Json;

namespace CsEvaluator.Engine.ReflectionEvaluator.Rules
{
    public class RuleEvaluation
    {
        [JsonConstructor]
        private RuleEvaluation() { }

        public RuleEvaluation(Rule rule, bool hasPassed, string reason = null)
        {
            Rule = rule;
            HasPassed = hasPassed;
            Reason = reason;
        }

        public Rule Rule { get; set; }
        public bool HasPassed { get; set; }
        public string Reason { get; set; }
    }
}
