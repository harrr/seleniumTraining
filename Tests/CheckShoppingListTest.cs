using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using SeleniumExtras.WaitHelpers;

namespace SelenuimInitial.Tests
{
    [TestFixture]
    public class CheckShoppingListTest : TestBase
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        [SetUp]
        public override void Start()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--no-sandbox");
            _driver = new ChromeDriver(options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        }
        [Test]
        public void RunCheckShoppingListTest()
        {
            try
            {
                _driver.Navigate().GoToUrl("http://localhost/litecart/");
                string ducksSelector = "//div[@id = 'box-most-popular']//div[@class = 'content']//ul//li//a[@class='link' and not(@title = 'Yellow Duck')]";
                ReadOnlyCollection<IWebElement> ducksToAdd = _driver.FindElements(By.XPath(ducksSelector));
                for (int i = 0; i < 3; i++)
                {
                    ducksToAdd[i].Click();
                    _wait.Until(d => d.FindElement(By.Name("add_cart_product")));
                    _driver.FindElement(By.Name("add_cart_product")).Click();
                    _driver.FindElement(By.CssSelector("#breadcrumbs>ul>li>a")).Click();
                    IWebElement quantity = _driver.FindElement(By.CssSelector("span.quantity"));
                    _wait.Until(ExpectedConditions.TextToBePresentInElement(quantity, (i+1).ToString()));
                    ducksToAdd = _driver.FindElements(By.XPath(ducksSelector));
                }
                _driver.FindElement(By.Id("cart")).Click();
                _wait.Until(d => d.FindElement(By.Name("remove_cart_item")));
                for (int i = 0; i < 3; i++)
                {
                    IWebElement removeButton = _driver.FindElement(By.Name("remove_cart_item"));
                    _wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("remove_cart_item")));
                    removeButton.Click();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [TearDown]
        public override void stop()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}
