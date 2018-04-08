using System;
using System.Linq;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using EvaluatorEngine.util;
using EvaluatorEngine.FileParser;
using EvaluatorEngine.ReflectionEvaluator.Rules;
using System.Threading.Tasks;

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

            //----reflection evaluation -----//

            var xmlParser = new XmlParser();
            var list = xmlParser.ParseToList(@"C:\Users\thinkpad-e560\Documents\Visual Studio 2017\Projects\cs-evaluator\EvaluatorEngine.Tests\Demo XML\DemoProiect.xml");

            int counter = 0;
            foreach (Rule rule in list)
            {
                counter += rule.Evaluate(assembly) == true ? 1 : 0;
            }


        }

        private BuildInfo BuildAndScan(string csFileFullName, string dllFileFullName)
        {
            var process = ProcessFactory.BuildAndScanProcess($"\"{csFileFullName}\" \"{dllFileFullName}\"");

            try
            {
                process.Start();

                string output = process.StandardOutput.ReadToEnd();

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
