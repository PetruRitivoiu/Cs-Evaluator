using CsEvaluator.Engine.ReflectionEvaluator.Enums;
using Newtonsoft.Json;

namespace CsEvaluator.Engine.ReflectionEvaluator.Rules
{
    public class RuleInfo
    {
        [JsonConstructor]
        private RuleInfo() { }

        public RuleInfo(Rule rule)
        {
            Id = rule.Id;
            SubjectType = rule.SubjectType;
            SubjectValue = rule.SubjectValue;
            Verb = rule.Verb;
            ComplementType = rule.ComplementType;
            ComplementValue = rule.ComplementValue;
            Count = rule.Count;

            Description = rule.ToString();
        }

        public int Id { get; set; }

        public SubjectType SubjectType { get; set; }
        public string SubjectValue { get; set; }
        public Verb Verb { get; set; }
        public ComplementType ComplementType { get; set; }
        public string ComplementValue { get; set; }
        public int Count { get; set; }

        public string Description { get; set; }

    }
}
