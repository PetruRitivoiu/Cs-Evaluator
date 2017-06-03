using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace EvaluatorEngine
{
    public class BPC : IEvaluator
    {

        private static string COMPILE_AND_EXECUTE = @"C:\Users\thinkpad-e560\Documents\Visual Studio 2017\Projects\cs-evaluator\EvaluatorEngine\CompileAndExecute.bat";
        private static string WORKING_DIRECTORY = @"C:\Users\thinkpad-e560\Documents\Visual Studio 2017\Projects\cs-evaluator\EvaluatorEngine";

        public Evaluation Evaluate(string[] args)
        {
            //relative paths
            //Arg 1 -> pathToFile (CS file)
            //Arg 2 -> exeFile  (CS file after compile and build)
            //Arg 3 -> validationFile
            //Arg 4 -> expectedFile

            Process myProcess = Process.Start(getProcessStartInfo(args));

            myProcess.Start();

            string result = myProcess.StandardOutput.ReadToEnd();

            string stderror = myProcess.StandardError.ReadToEnd();

            myProcess.WaitForExit();

            if (!String.IsNullOrEmpty(stderror))
            {
                return new Evaluation(-1, stderror);
            }
            else
            {
                string[] arrResult = splitString(result, Environment.NewLine);

                return CompareResults(arrResult, WORKING_DIRECTORY + @"\" + args[3]);
            }
        }

        private Evaluation CompareResults(string[] result, string expectedFile)
        {
            string[] expected;
            string stdErr = "";

            using (StreamReader sr = File.OpenText(expectedFile))
            {
                var list = new List<string>();

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    list.Add(line);
                }
                expected = list.ToArray();
            }


            var resultAndExpected = result.Zip(expected, (r, e) => new { Result = r, Expected = e });

            int evaluationResult = expected.Length;

            if (result.Length != expected.Length)
            {
                stdErr = "output and expected string arrays don't have the same number of lines";
                evaluationResult = -1;
            }
            else
            {
                foreach (var re in resultAndExpected)
                {
                    if (!re.Expected.Equals(re.Result))
                    {
                        evaluationResult--;
                    }
                }
            }

            if (evaluationResult != -1) {
            evaluationResult = evaluationResult / expected.Length * 10;
            }

            return new Evaluation(evaluationResult, stdErr);
        }

        private ProcessStartInfo getProcessStartInfo(string[] args)
        {
            ProcessStartInfo psi = new ProcessStartInfo();

            psi.WorkingDirectory = WORKING_DIRECTORY;
            psi.FileName = "\"" + COMPILE_AND_EXECUTE + "\"";

            psi.Arguments = args[0] + " " + args[1] + " " + args[2];
            psi.UseShellExecute = false;

            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            return psi;
        }

        private string[] splitString(string result, string separator)
        {
            string[] arrResult = result.Split(new string[] { separator }, StringSplitOptions.None);
            if (String.IsNullOrEmpty(arrResult[arrResult.Length - 1]))
            {
                arrResult = arrResult.Take(arrResult.Length - 1).ToArray();
            }

            return arrResult;
        }
    }
}
