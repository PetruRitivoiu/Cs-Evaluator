using NUnit.Framework;

namespace DemoProiectPAW
{
    [TestFixture]
    public class MockUnitTestingFile
    {
        [Test]
        public void MockTestFalse()
        {
            Assert.AreEqual(1, 2);
        }

        [Test]
        public void MockTestTrue()
        {
            Assert.AreEqual(1, 1);
        }

        [Test]
        [Ignore("mock")]
        public void MockTestIgnore()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
