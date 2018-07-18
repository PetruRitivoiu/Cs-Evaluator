using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using CsEvaluator.Engine.Common;

namespace CsEvaluator.Engine
{
    public static class EvaluatorTaskFactory
    {
        public static readonly TimeSpan MaxWait;

        private static int TaskCounter;

        private static int NumberOfProcessors;

        private static ConcurrentQueue<EvaluationTask> TaskQueue;

        private static AutoResetEvent ReadyToDeque;

        static EvaluatorTaskFactory()
        {
            MaxWait = TimeSpan.FromMilliseconds(5000);
            TaskCounter = 0;
            NumberOfProcessors = 4;
            TaskQueue = new ConcurrentQueue<EvaluationTask>();
            ReadyToDeque = new AutoResetEvent(false);
        }

        public static Task<Evaluation> CreateAndStart(EvaluationTask evaluationTask)
        {
            if (Thread.VolatileRead(ref TaskCounter) >= NumberOfProcessors)
            {
                TaskQueue.Enqueue(evaluationTask);
                ReadyToDeque.WaitOne(MaxWait);
                TaskQueue.TryDequeue(out evaluationTask);

                //preia EvaluationTask-ul din eveniment
                EvaluationTask currentTask;
                TaskQueue.TryPeek(out currentTask);

                lock(TaskQueue)
                {
                    if (evaluationTask == currentTask)
                    {
                        TaskQueue.TryDequeue(out evaluationTask);
                        //continue..
                    }
                }

            }

            Interlocked.Increment(ref TaskCounter);

            var pawEvaluator = new PAWEvaluator();

            Task<Evaluation> T = new Task<Evaluation>(() => pawEvaluator.Evaluate(
                            evaluationTask.WorkingDirectory,
                            evaluationTask.ShortFileName,
                            evaluationTask.ReflectionFile,
                            evaluationTask.UnitTestingFile));


            T.ContinueWith((evaluation) => UpdateResults(evaluation));

            T.Start();

            return T;
        }

        private static int UpdateResults(Task<Evaluation> evaluation)
        {
            Interlocked.Decrement(ref TaskCounter);

            if (TaskCounter < NumberOfProcessors)
            {
                ReadyToDeque.Set();
            }

            return TaskCounter;
        }
    }

}
