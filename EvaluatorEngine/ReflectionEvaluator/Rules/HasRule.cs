using EvaluatorEngine.ReflectionEvaluator.Enums;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

namespace EvaluatorEngine.ReflectionEvaluator.Rules
{
    public class HasRule : Rule
    {
        public override bool Evaluate(Assembly assembly)
        {
            return false;
        }
    }
}
