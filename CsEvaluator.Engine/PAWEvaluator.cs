using System;
using System.Linq;
using System.Reflection;
using System.IO;
using CsEvaluator.Engine.Util;
using CsEvaluator.Engine.FileParser;
using CsEvaluator.Engine.ReflectionEvaluator.Rules;
using System.Collections.Generic;

namespace CsEvaluator.Engine
{
    public class PAWEvaluator : IEvaluator
    {
        public PAWEvaluator()
        {
            Directory.CreateDirectory(Config.BasePathToCodeFiles);
            Directory.CreateDirectory(Config.BasePathToDllFiles);
            Directory.CreateDirectory(Config.BasePathToValidationFiles);
        }

        public Evaluation Evaluate(string shortFileName, string fullReflectionFile)
        {
            var csFileFullName = Path.Combine(Config.BasePathToCodeFiles, shortFileName);
            var dllFileFullName = Path.Combine(Config.BasePathToDllFiles, shortFileName);
            dllFileFullName = UtilHelper.ChangeVirtualExtension(dllFileFullName, ".dll");

            var buildInfo = BuildAndScan(csFileFullName, dllFileFullName);

            if (!buildInfo.Succes)
            {
                return new Evaluation(buildInfo.Info);
            }

            if (!ScanForViruses(UtilHelper.RemoveVirtualExtension(shortFileName)))
            {
                return new Evaluation("dll file contains viruses");
            }

            var assembly = Assembly.LoadFile(dllFileFullName);

            //----reflection evaluation -----//

            Evaluation evaluation = ReflectionEvaluation(assembly, fullReflectionFile);

            return evaluation;
        }

        private Evaluation ReflectionEvaluation(Assembly assembly, string fullReflectionFile)
        {
            IParser xmlParser = new XmlParser();
            var rulesList = xmlParser.ParseToList(fullReflectionFile);

            int counter = 0;
            var rulesEvaluation = new List<RuleEvaluation>();
            foreach (Rule rule in rulesList)
            {
                var result = rule.Evaluate(assembly);
                rulesEvaluation.Add(result);
                counter += result.HasPassed == true ? 1 : 0;
            }

            double studentsMark = (double)counter / rulesList.Count() * 10;

            return new Evaluation(studentsMark, rulesEvaluation);
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

            switch (file.Extension)
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
