using Newtonsoft.Json;

namespace CsEvaluator.Engine.ReflectionEvaluator.Rules
{
    public class RuleEvaluation
    {
        [JsonConstructor]
        private RuleEvaluation() { }

        public RuleEvaluation(RuleInfo ruleInfo, bool hasPassed, string reason = null)
        {
            RuleInfo = ruleInfo;
            HasPassed = hasPassed;
            Reason = reason;
        }

        public RuleInfo RuleInfo { get; set; }
        public bool HasPassed { get; set; }
        public string Reason { get; set; }
    }
}
