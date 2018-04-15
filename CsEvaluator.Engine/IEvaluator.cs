namespace CsEvaluator.Engine
{
    public interface IEvaluator
    {
        Evaluation Evaluate(string shortFileName, string shortValidationFileName);
    }
}
