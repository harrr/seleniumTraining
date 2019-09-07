using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SelenuimInitial
{
    [TestFixture]
    public abstract class TestBase
    {
        protected IWebDriver _driver;
        protected WebDriverWait _wait;

        [SetUp]
        public abstract void Start();
        
        [TearDown]
        protected void stop(IWebDriver driver)
        {
            driver.Quit();
            driver = null;
        }
    }
}
