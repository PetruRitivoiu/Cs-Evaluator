using CsEvaluator.Engine.Common;

namespace CsEvaluator.Engine
{
    public interface IEvaluator
    {
        Evaluation Evaluate(string workingDirectory, string shortFileName, string fullReflectionFile, string fullUnitTestingFile);
    }
}
