using CsEvaluator.Engine.ReflectionEvaluator.Enums;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace CsEvaluator.Engine.ReflectionEvaluator.Rules
{
    public abstract class Rule
    {
        public int Id { get; set; }

        public SubjectType SubjectType { get; set; }
        public string SubjectValue { get; set; }
        public Verb Verb { get; set; }
        public ComplementType ComplementType { get; set; }
        public string ComplementValue { get; set; }
        public int Count { get; set; }

        protected static string NullOrDefault = "NullOrDefault";

        public abstract RuleEvaluation Evaluate(Assembly assembly);

        //util
        protected bool IsNullOrDefault(string s)
        {
            return string.Compare(s, Rule.NullOrDefault, true) == 0
                || string.IsNullOrWhiteSpace(s);
        }

        public static Rule CreateByVerb(Verb verb)
        {
            switch (verb)
            {
                case Verb.HAS:
                    return new HasRule();

                case Verb.IS:
                    return new IsRule();

                default:
                    return null;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            bool andFlag = false;
            bool verbFlag = true;

            sb.Append($"Defined members of type {SubjectType}");

            if (!IsNullOrDefault(SubjectValue))
            {
                sb.Append($" named {SubjectValue}");
            }

            //flags
            if (ComplementType != ComplementType.NullOrDefault || !IsNullOrDefault(ComplementValue))
            {
                andFlag = true;
            }
            if (ComplementType == ComplementType.NullOrDefault && IsNullOrDefault(ComplementValue))
            {
                verbFlag = false;
            }

            if (verbFlag)
            {
                sb.Append(Verb == Verb.HAS ? " HAVE" : " ARE");
            }

            if (ComplementType != ComplementType.NullOrDefault)
            {
                if (verbFlag && Verb == Verb.IS)
                {
                    sb.Append(" OF TYPE");
                }
                sb.Append($" {ComplementType}");
            }

            if (!IsNullOrDefault(ComplementValue))
            {
                if (Verb == Verb.HAS)
                {
                    sb.Append($" named {ComplementValue}");
                }
                else
                {
                    sb.Append($" OF TYPE {ComplementValue}");
                }
            }

            if (andFlag)
            {
                sb.Append(" AND");
            }

            sb.Append($" count in total at least {Count}");

            return sb.ToString();
        }

    }
}
