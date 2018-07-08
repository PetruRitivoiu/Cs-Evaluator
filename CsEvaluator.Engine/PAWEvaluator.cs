using System;
using System.Linq;
using System.Reflection;
using System.IO;
using CsEvaluator.Engine.Util;
using CsEvaluator.Engine.FileParser;
using CsEvaluator.Engine.ReflectionEvaluator.Rules;
using CsEvaluator.Engine.Common;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Xml.Linq;

namespace CsEvaluator.Engine
{
    public class PAWEvaluator : IEvaluator
    {
        public PAWEvaluator()
        {
            Directory.CreateDirectory(Config.BasePathToCodeFiles);
            Directory.CreateDirectory(Config.BasePathToValidationFiles);
        }

        public Evaluation Evaluate(string workingDirectory, string shortFileName, string fullReflectionFile, string fullUnitTestingFile)
        {
            var dllFileFullName = Path.Combine(workingDirectory, shortFileName);
            dllFileFullName = UtilHelper.ChangeVirtualExtension(dllFileFullName, ".dll");

            var buildInfo = BuildAndScan(workingDirectory, dllFileFullName, fullUnitTestingFile);

            if (!buildInfo.Succes)
            {
                return new Evaluation(buildInfo.Info);
            }

            if (!ScanForViruses(UtilHelper.RemoveVirtualExtension(shortFileName)))
            {
                return new Evaluation("dll file contains viruses");
            }

            byte[] assemblyBytes = File.ReadAllBytes(dllFileFullName);
            var assembly = Assembly.Load(assemblyBytes);

            //----static evaluation -----//

            var staticEvaluation = RunStaticEvaluation(assembly, fullReflectionFile);

            //----functional evaluation -----//

            var functionalEvaluation = RunFunctionalEvaluation(assembly, workingDirectory, dllFileFullName);

            return new Evaluation(staticEvaluation, functionalEvaluation);
        }

        private StaticEvaluation RunStaticEvaluation(Assembly assembly, string fullReflectionFile)
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

            return new StaticEvaluation(studentsMark, rulesEvaluation);
        }

        private FunctionalEvaluation RunFunctionalEvaluation(Assembly assembly, string workingDirectory, string fullUnitTestingFile)
        {
            var process = ProcessFactory.RunUnitTests(workingDirectory, fullUnitTestingFile);

            try
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    //to-do
                }

                if (string.IsNullOrEmpty(output))
                {
                    //to-do
                }
                else
                {
                    //to-do
                }

            }
            catch(Exception ex)
            {
                //to-do
            }

            return null;
        }

        private BuildInfo BuildAndScan(string workingDirectory, string dllFileFullName, string unitTestingFileFullName)
        {
            //Add unit testing file to .csproj file
            var csProjPath = Directory.GetFiles(workingDirectory, "*.csproj").FirstOrDefault();
            var shortTestFileName = unitTestingFileFullName.Split('\\').Last();

            var doc = XDocument.Load(csProjPath);
            var ns = doc.Root.GetDefaultNamespace();

            var element = new XElement(ns + "Compile", new XAttribute("Include", shortTestFileName));

            if (doc.Root.Descendants().Any(d => d.Name.LocalName == "Compile" && d.Attribute("Include") != null))
            {
                if (!doc.Root.Descendants().Any(d => d.Name.LocalName == "Compile" && d.Attribute("Include").Value == shortTestFileName))
                {
                    doc.Root.Descendants().Where(d => d.Name.LocalName == "Compile" && d.Attribute("Include") != null).FirstOrDefault().Parent.Add(element);
                }
            }

            doc.Save(csProjPath);

            //Build the assembly
            var process = ProcessFactory.BuildAndScanProcess(workingDirectory, dllFileFullName);

            try
            {
                process.Start();

                string output = process.StandardOutput.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    return new BuildInfo(false, "Exit code not 0");
                }

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
            var files = Directory.EnumerateFiles(Config.BasePathToCodeFiles, $"{shortFileName}.*", SearchOption.AllDirectories)
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
