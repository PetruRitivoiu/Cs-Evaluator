using EvaluatorEngine.FileParser;
using EvaluatorEngine.ReflectionEvaluator.Rules;
using NUnit.Framework;
using System;
using System.Reflection;
using System.Threading;

namespace EvaluatorEngine.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestBuildAndScan()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            Console.WriteLine("Hello World!");

            PAWEvaluator p = new PAWEvaluator();
            p.Evaluate("DemoProiectCS", "asdf.txt");

            watch.Stop();

            Console.WriteLine($"Tests.Main total time elapsed: {watch.ElapsedMilliseconds}");

            Assert.AreEqual(1, 1);
        }

        [Test]
        public void TestTaskEvaluation()
        {
            var a = TaskFactory.CreateAndStart("DemoProiectCS", "asdf.txt");
            var b = TaskFactory.CreateAndStart("DemoProiectCS", "asdf.txt");
            var c = TaskFactory.CreateAndStart("DemoProiectCS", "asdf.txt");

            a.Wait();
            b.Wait();
            c.Wait();

            Assert.AreEqual(1, 1);
        }

        [Test]
        public void TestXmlParser()
        {
            var xml = new XmlParser();
            var rules = xml.ParseToList(@"C:\Users\thinkpad-e560\Documents\Visual Studio 2017\Projects\cs-evaluator\EvaluatorEngine.Tests\MockData\MockProiect.xml");

            Assert.AreEqual(rules.Count, 11);
        }

        [Test]
        public void TestRules()
        {
            var xmlParser = new XmlParser();

            var list =
                xmlParser.ParseToList(@"C:\Users\thinkpad-e560\Documents\Visual Studio 2017\Projects\cs-evaluator\EvaluatorEngine.Tests\MockData\MockProiect.xml");

            var assembly =
                Assembly.LoadFrom(@"C:\Users\thinkpad-e560\Documents\Visual Studio 2017\Projects\cs-evaluator\EvaluatorEngine.Tests\MockData\MockProiectCS.dll");

            int counter = 0;
            foreach (Rule rule in list)
            {
                counter += rule.Evaluate(assembly) == true ? 1 : 0;
            }

            Assert.AreEqual(4, counter);
        }

    }
}