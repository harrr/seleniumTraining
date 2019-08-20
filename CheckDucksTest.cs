using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelenuimInitial
{
    [TestFixture]
    public class CheckDucksTest
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        [SetUp]
        public void start()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("no-sandbox");
            _driver = new ChromeDriver(options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        }
        [Test]
        public void DucksChromeTest()
        {
            try
            {
                _driver.Navigate().GoToUrl("http://localhost/litecart");
                var productItems = _driver.FindElements(By.XPath("//ul[@class='listing-wrapper products']//li"));
                foreach (IWebElement productItem in productItems)
                {
                    CheckProductItem(productItem);
                }
            }
            catch (Exception ex)
            {
                stop();
                throw ex;
            }
        }
        private void CheckProductItem(IWebElement element)
        {
            string elementName = element.FindElement(By.ClassName("name")).Text;
            try
            {
                IWebElement sticker = element.FindElement(By.CssSelector("div[class^='sticker']"));
                Console.WriteLine($"Product {elementName} has a sticker \"{sticker.Text}\"");
            }
            catch
            {
                Console.WriteLine($"Product {elementName} has no sticker.");
            }
        }
        [TearDown]
        public void stop()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}
