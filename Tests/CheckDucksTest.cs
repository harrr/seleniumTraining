using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                ReadOnlyCollection<IWebElement> stickerElements = element.FindElements(By.CssSelector("div[class^='sticker']"));
                int n = stickerElements.Count;
                switch(n)
                {
                    case 0:
                        Console.WriteLine($"Product {elementName} has no stickers.");
                        break;
                    case 1:
                        Console.WriteLine($"Product {elementName} has a sticker.");
                        break;
                    default:
                        throw new Exception($"Product {elementName} has more than 1 sticker.");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
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
