using System.Threading;
using System.Threading.Tasks;
using CsEvaluator.Engine.Common;

namespace CsEvaluator.Engine
{
    public class EvaluatorTaskFactory
    {
        public static int TaskCounter = 0;

        public static Task<Evaluation> CreateAndStart(string workingDirectory, string shortFileName, string reflectionFile, string unitTestingFile)
        {
            Interlocked.Increment(ref TaskCounter);

            var pawEvaluator = new PAWEvaluator();

            Task<Evaluation> T = new Task<Evaluation>(() => pawEvaluator.Evaluate(workingDirectory, shortFileName, reflectionFile, unitTestingFile));

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
