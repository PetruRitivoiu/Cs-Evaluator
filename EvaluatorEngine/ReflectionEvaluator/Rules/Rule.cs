﻿using EvaluatorEngine.ReflectionEvaluator.Enums;
using System.Reflection;

namespace EvaluatorEngine.ReflectionEvaluator.Rules
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

        public abstract bool Evaluate(Assembly assembly);

        protected bool IsNullOrDefault(string s)
        {
            return string.Compare(SubjectValue, Rule.NullOrDefault, true) != 0
                || !string.IsNullOrWhiteSpace(SubjectValue);
        }
    }
}
