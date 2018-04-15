using CsEvaluator.Engine.ReflectionEvaluator.Rules;
using System.Collections.Generic;

namespace CsEvaluator.Engine.FileParser
{
    public interface IParser
    {
        List<Rule> ParseToList(string fullPath);
    }
}
