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

            //Process process = new Process();

            ProcessStartInfo psi = new ProcessStartInfo();

            string fullPath = @"C:\Users\thinkpad-e560\Documents\Visual Studio 2017\Projects\cs-evaluator\EvaluatorEngine\CompileAndExecute.bat";
            psi.FileName = "\"" + fullPath + "\"";
            //psi.WorkingDirectory = Path.GetDirectoryName(fullPath);
            psi.Arguments = arg;
            psi.UseShellExecute = false;

            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.Arguments = arg;

            //process.StartInfo.RedirectStandardOutput = true;
            //process.StartInfo.RedirectStandardError = true;
            //process.StartInfo.Arguments = arg;

            Process myProcess = Process.Start(psi);
            myProcess.Start();

            //* Read the output (or the error)
            string output = myProcess.StandardOutput.ReadToEnd();
            result = output;

            string err = myProcess.StandardError.ReadToEnd();
            stderror = err;

            if (!String.IsNullOrEmpty(err))
            {
                output += Environment.NewLine + err;
            }

            myProcess.WaitForExit();

            return output;
        }

    }
}
