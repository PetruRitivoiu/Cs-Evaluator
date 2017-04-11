using System;
using System.Diagnostics;
using System.IO;

namespace EvaluatorEngine
{
    public class BPC : IEvaluator
    {
        
        public string Evaluate(string arg)
        {
            string result;
            string stderror;

            Process process = new Process();
            process.StartInfo.FileName = @"C:\Users\thinkpad-e560\Documents\Visual Studio 2017\Projects\cs-evaluator\EvaluatorEngine" + @"\CompileAndExecute.bat";
            process.StartInfo.Arguments = arg;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();

            //* Read the output (or the error)
            string output = process.StandardOutput.ReadToEnd();
            result = output;

            string err = process.StandardError.ReadToEnd();
            stderror = err;
            process.WaitForExit();

            return output + "--" + stderror;
        }

    }
}
