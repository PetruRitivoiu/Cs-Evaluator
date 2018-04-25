using CsEvaluator.Engine;
using CsEvaluator.Engine.FileParser;
using CsEvaluator.Engine.ReflectionEvaluator.Rules;
using NUnit.Framework;
using System;
using System.Reflection;

namespace CsEvaluator.Engine.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestBuildAndScan()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            Console.WriteLine("Hello World!");

            IEvaluator p = new PAWEvaluator();
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

        [TestCase(10)]
        public void TestXmlParser(int expected)
        {
            var xml = new XmlParser();
            var rules = xml.ParseToList(@"C:\Users\thinkpad-e560\Documents\Visual Studio 2017\Projects\cs-evaluator\CsEvaluator.Engine.Tests\MockData\MockProiect.xml");

            Assert.AreEqual(expected, rules.Count);
        }

        [TestCase(10)]
        public void TestRules(int expected)
        {
            var xmlParser = new XmlParser();

            var list =
                xmlParser.ParseToList(@"C:\Users\thinkpad-e560\Documents\Visual Studio 2017\Projects\cs-evaluator\CsEvaluator.Engine.Tests\MockData\MockProiect.xml");

            var assembly =
                Assembly.LoadFrom(@"C:\Users\thinkpad-e560\Documents\Visual Studio 2017\Projects\cs-evaluator\CsEvaluator.Engine.Tests\MockData\MockProiectCS.dll");

            int count = 0;
            foreach (Rule rule in list)
            {
                count += rule.Evaluate(assembly).HasPassed == true ? 1 : 0;
            }

            Assert.AreEqual(expected, count);
        }

    }
}