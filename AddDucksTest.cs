using NUnit.Framework;

namespace ShoppingListPatterns
{
    [TestFixture]
    public class AddDucksTest
    {
        private LiteCardApp LiteCardApp;

        [SetUp]
        public void Start()
        {
            LiteCardApp = new LiteCardApp();
        }
        [Test]
        public void RunAddDucksTest()
        {
            LiteCardApp.OpenCatalog();
            LiteCardApp.AddDucks(3);
            LiteCardApp.OpenCart();
            LiteCardApp.DeleteDucks(3);
        }
        [TearDown]
        public void stop()
        {
            LiteCardApp.Quit();
        }
    }
}
