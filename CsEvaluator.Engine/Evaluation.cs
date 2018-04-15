namespace CsEvaluator.Engine
{
    public class Evaluation
    {
        public Evaluation(int evaluationResult, string stdError)
        {
            this.EvaluationResult = evaluationResult;
            this.StdError = stdError;
        }

        public double EvaluationResult { get; }

        public string StdError { get; }
    }
}
