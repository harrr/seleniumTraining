using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;

namespace SelenuimInitial.Tests
{
    [TestFixture]
    public class CheckLogsTest : TestBase
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
        public void RunCheckLogsTest()
        {
            try
            {
                _driver.Navigate().GoToUrl("http://localhost/litecart/admin/");
                var loginField = _driver.FindElement(By.Name("username"));
                loginField.SendKeys("admin");
                var passwordField = _driver.FindElement(By.Name("password"));
                passwordField.SendKeys("admin");
                var loginButton = _driver.FindElement(By.Name("login"));
                loginButton.Click();
                IWebElement mainMenuElement = _driver.FindElement(By.Id("box-apps-menu"));
                var menuItems = mainMenuElement.FindElements(By.XPath("//li"));
                GetMenuItemByName("Catalog", menuItems).Click();
                var logs = _driver.Manage().Logs;
                var logTypes = _driver.Manage().Logs.AvailableLogTypes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private IWebElement GetMenuItemByName(string name, ReadOnlyCollection<IWebElement> menuItems)
        {
            foreach (IWebElement item in menuItems)
            {
                IWebElement itemName = item.FindElement(By.XPath("./a/span[@class='name']"));
                if (itemName.GetAttribute("innerText").Equals(name))
                {
                    return item;
                }
            }
            return null;
        }
        [TearDown]
        public override void stop()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}
