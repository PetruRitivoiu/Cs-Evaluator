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

        [Test]
        public void TestUrlValid()
        {
            var demoTesting = new DemoTesting();

            var url = @"https://www.google.ro/";

            var result = demoTesting.IsURLValid(url);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void TestUrlInvalid()
        {
            var demoTesting = new DemoTesting();

            var url = @"https://www.google123xyz.ro/";

            var result = demoTesting.IsURLValid(url);

            Assert.AreEqual(false, result);
        }
    }
}
