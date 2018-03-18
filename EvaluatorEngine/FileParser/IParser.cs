using EvaluatorEngine.ReflectionEvaluator.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvaluatorEngine.FileParser
{
    public interface IParser
    {
        IList<Rule> ParseToList(string fullPath);
    }
}
