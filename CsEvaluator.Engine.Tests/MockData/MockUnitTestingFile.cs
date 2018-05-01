using NUnit.Framework;

namespace CsEvaluator.Engine.Tests
{
    [TestFixture]
    public class MockUnitTestingFile
    {
        [Test]
        [Ignore("mock")]
        public void MockTestFalse()
        {
            Assert.AreEqual(1, 2);
        }

        [Test]
        [Ignore("mock")]
        public void MockTestTrue()
        {
            Assert.AreEqual(1, 1);
        }
    }
}