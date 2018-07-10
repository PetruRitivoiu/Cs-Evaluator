using NUnit.Framework;

namespace DemoProiectPAW
{
    [TestFixture]
    public class UnitTesting
    {
        [Test]
        public void TestFilename()
        {
            var demoTesting = new DemoTesting();

            var filename = @"C:\Users\thinkpad-e560\Documents\Visual Studio 2017\Projects\DemoProiectPAW\DemoProiectPAW";

            var result = demoTesting.GetShortFileName(filename);

            Assert.AreEqual("DemoProiectPAW", result);
        }

        [TestCase("https://www.google.ro", true)]
        [TestCase("https://www.asdf1234.xyz", false)]
        public void TestUrlValid(string url, bool expected)
        {
            var demoTesting = new DemoTesting();

            var result = demoTesting.IsURLValid(url);

            Assert.AreEqual(expected, result);
        }
    }
}
