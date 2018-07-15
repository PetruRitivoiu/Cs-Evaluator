using System;
using System.Collections.Generic;
using System.Text;

namespace CsEvaluator.Engine
{
    public class EvaluationTask
    {
        public EvaluationTask(string workingDirectory, string shortFileName, string reflectionFile, string unitTestingFile)
        {
            WorkingDirectory = workingDirectory;
            ShortFileName = shortFileName;
            ReflectionFile = reflectionFile;
            UnitTestingFile = unitTestingFile;
        }

        public string WorkingDirectory { get; }
        public string ShortFileName { get; }
        public string ReflectionFile { get; }
        public string UnitTestingFile { get; }
    }
}
