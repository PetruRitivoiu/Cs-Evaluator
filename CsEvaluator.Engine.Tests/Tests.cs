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
        [TestCase(10)]
        public void TestXmlParser(int expected)
        {
            var xml = new XmlParser();
            var rules = xml.ParseToList(@"C:\Users\thinkpad-e560\Documents\Visual Studio 2017\Projects\cs-evaluator\CsEvaluator.Engine.Tests\MockData\MockReflectionFile.xml");

            Assert.AreEqual(expected, rules.Count);
        }

        [TestCase(8)]
        public void TestReflectionValidation(int expected)
        {
            var xmlParser = new XmlParser();

            var list =
                xmlParser.ParseToList(@"C:\Users\thinkpad-e560\Documents\Visual Studio 2017\Projects\cs-evaluator\CsEvaluator.Engine.Tests\MockData\MockReflectionFile.xml");

            var assembly =
                Assembly.LoadFrom(@"C:\Users\thinkpad-e560\Documents\Visual Studio 2017\Projects\cs-evaluator\CsEvaluator.Engine.Tests\MockData\MockProiect.exe");

            int count = 0;
            foreach (Rule rule in list)
            {
                count += rule.Evaluate(assembly).HasPassed == true ? 1 : 0;
            }

            Assert.AreEqual(expected, count);
        }

        [Test]
        public void TestUnitTesting()
        {
            Assert.Fail();
        }

    }
}