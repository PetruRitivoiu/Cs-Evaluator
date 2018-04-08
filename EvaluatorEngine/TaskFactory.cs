﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EvaluatorEngine
{
    public class TaskFactory
    {
        public static int TaskCounter = 0;

        public static Task CreateAndStart(string shortFileName, string shortValidationFileName)
        {
            Interlocked.Increment(ref TaskCounter);

            var pawEvaluator = new PAWEvaluator();

            Task<Evaluation> T = new Task<Evaluation>(() => pawEvaluator.Evaluate(shortFileName, shortValidationFileName));

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
