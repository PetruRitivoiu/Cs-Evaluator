using System;
using System.Linq;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using EvaluatorEngine.util;

namespace EvaluatorEngine
{
    public class PAWEvaluator : IEvaluator
    {
        public Evaluation Evaluate(string shortFileName, string shortValidationFileName)
        {
            var csFileFullName = Path.Combine(Config.BasePathToCodeFiles, shortFileName);
            csFileFullName += ".cs";
            var dllFileFullName = Path.Combine(Config.BasePathToDllFiles, shortFileName);
            dllFileFullName += ".dll";

            var buildInfo = BuildAndScan(csFileFullName, dllFileFullName);

            if (!buildInfo.Succes)
            {
                return new Evaluation(-1, buildInfo.Info);
            }

            if (!ScanForViruses("DemoProiectCS"))
            {
                return new Evaluation(-1, "dll file contains viruses");
            }

            var assembly = Assembly.LoadFile(dllFileFullName);

            return null;
        }

        private BuildInfo BuildAndScan(string csFileFullName, string dllFileFullName)
        {
            var process = ProcessFactory.BuildAndScanProcess($"\"{csFileFullName}\" \"{dllFileFullName}\"");

            try
            {
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                Console.WriteLine(output);

                process.WaitForExit();

                if (string.IsNullOrEmpty(output))
                {
                    return new BuildInfo(true, "Build succeeded");
                }
                else
                {
                    return new BuildInfo(false, output);
                }

            }

            catch (Exception ex)
            {
                return new BuildInfo(false, $"An exception occured while trying to build and scan the .cs file \r\n {ex}");
            }

        }

        private bool ScanForViruses(string shortFileName)
        {
            var files = Directory.EnumerateFiles(Config.BasePathToDllFiles, $"{shortFileName}.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".dll") || s.EndsWith(".vir"));

            var file = new FileInfo(files.FirstOrDefault());

            switch(file.Extension)
            {
                case ".vir": 
                    return false;

                case ".dll":
                    return true;

                default:
                    //throw exception;
                    return false;
            }
        }

    }
}
