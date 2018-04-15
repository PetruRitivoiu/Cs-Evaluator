using System.Diagnostics;

namespace CsEvaluator.Engine.Util
{
    public static class ProcessFactory
    {
        public static Process BuildAndScanProcess(string args)
        {
                var process = new Process();
                process.StartInfo.FileName = @"C:\Users\thinkpad-e560\Documents\Visual Studio 2017\Projects\cs-evaluator\EvaluatorEngine\BuildAndScan.bat";
                process.StartInfo.Arguments = args;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                return process;
        }
    }
}
