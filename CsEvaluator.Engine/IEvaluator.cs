namespace CsEvaluator.Engine
{
    public interface IEvaluator
    {
        Evaluation Evaluate(string workingDirectory, string shortFileName, string fullReflectionFile);
    }
}
