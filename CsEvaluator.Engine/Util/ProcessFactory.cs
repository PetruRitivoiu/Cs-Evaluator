using System.Diagnostics;

namespace CsEvaluator.Engine.Util
{
    public static class ProcessFactory
    {
        public static Process BuildAndScanProcess(string workingDirectory, string args)
        {
            var process = new Process();
            process.StartInfo.WorkingDirectory = workingDirectory;
            process.StartInfo.FileName = Config.PathToBuildAndScanScript;
            process.StartInfo.Arguments = $" \"{args}\" ";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            return process;
        }

        public static Process RunUnitTests(string workingDirectory, string args)
        {
            var process = new Process();
            process.StartInfo.WorkingDirectory = workingDirectory;
            process.StartInfo.FileName = Config.PathToUnitTestingScript;
            process.StartInfo.Arguments = $" \"{args}\" ";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            return process;
        }
    }
}
