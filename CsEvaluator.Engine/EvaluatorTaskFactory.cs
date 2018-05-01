using System.Threading;
using System.Threading.Tasks;

namespace CsEvaluator.Engine
{
    public class EvaluatorTaskFactory
    {
        public static int TaskCounter = 0;

        public static Task<Evaluation> CreateAndStart(string workingDirectory, string shortFileName, string shortValidationFileName)
        {
            Interlocked.Increment(ref TaskCounter);

            var pawEvaluator = new PAWEvaluator();

            Task<Evaluation> T = new Task<Evaluation>(() => pawEvaluator.Evaluate(workingDirectory, shortFileName, shortValidationFileName));

            T.ContinueWith((evaluation) => UpdateResults(evaluation));

            T.Start();

            return T;
        }

        private static int UpdateResults(Task<Evaluation> evaluation)
        {
            Interlocked.Decrement(ref TaskCounter);

            return TaskCounter;
        }
    }

}
