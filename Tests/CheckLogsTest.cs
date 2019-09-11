using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;

namespace SelenuimInitial.Tests
{
    [TestFixture]
    public class CheckLogsTest 
    {
        private ChromeDriver _driver;
        private WebDriverWait _wait;
        private IWebElement mainMenuElement;
        [SetUp]
        public void Start()
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
                mainMenuElement = _driver.FindElement(By.Id("box-apps-menu"));
                var menuItems = mainMenuElement.FindElements(By.XPath("//li"));
                GetMenuItemByName("Catalog", menuItems).Click();
                IWebElement rubberDucksForder = _driver.FindElement(By.XPath("//table[@class='dataTable']//tr[@class='row'][2]//a"));
                rubberDucksForder.Click();
                var duckItems = _driver.FindElements(By.XPath("//table[@class='dataTable']//tr[@class='row']//td//img//..//a"));
                for (int i = 0; i < duckItems.Count; i++)
                {
                    duckItems[i].Click();
                    CheckLogs();
                    _driver.Navigate().Back();
                    duckItems = _driver.FindElements(By.XPath("//table[@class='dataTable']//tr[@class='row']//td//img//..//a"));
                }
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

        //пыталась сделать через eventfiringwebdriver ElementClicked, 
        //но не получилось отписаться от события, и тест зацикливался
        private void ElementClickedEvent(object e, WebElementEventArgs sender)
        {
            CheckLogs();
            _driver.Navigate().Back();
        }
        private void CheckLogs()
        {
            var types = _driver.Manage().Logs.AvailableLogTypes; //возвращает "browser", "driver"
            foreach(var type in types)
            {
                var logs = _driver.Manage().Logs.GetLog(type); //всегда возвращает null
                if(logs.Count > 0)
                {
                    Console.WriteLine($"Found {logs.Count} of type {type}");
                    Console.WriteLine(string.Join("\n", logs));
                }
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
