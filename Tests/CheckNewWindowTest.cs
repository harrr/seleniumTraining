using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SelenuimInitial.Tests
{
    public class CheckNewWindowTest : TestBase
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
        public void RunCheckNewWindowTest()
        {
            try
            {
                _driver.Navigate().GoToUrl("http://localhost/litecart/admin");
                Login();
                IWebElement mainMenuElement = _driver.FindElement(By.Id("box-apps-menu"));
                var menuItems = mainMenuElement.FindElements(By.XPath("//li"));
                IWebElement countriesItem = GetMenuItemByName("Countries", menuItems);
                countriesItem.Click();
                IWebElement addNewCountryButton = _driver.FindElement(By.CssSelector("a.button"));
                addNewCountryButton.Click();
                ReadOnlyCollection<IWebElement> externalLinkElements = _driver.FindElements(By.CssSelector("i.fa-external-link"));
                foreach(IWebElement externalLink in externalLinkElements)
                {
                    string mainWindow = _driver.CurrentWindowHandle;
                    externalLink.Click();
                    string newWindow = _wait.Until(d => d.WindowHandles.FirstOrDefault(h=>!h.Equals(mainWindow)));
                    _driver.SwitchTo().Window(newWindow);
                    _driver.Close();
                    _driver.SwitchTo().Window(mainWindow);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Login()
        {
            var loginField = _driver.FindElement(By.Name("username"));
            loginField.SendKeys("admin");
            var passwordField = _driver.FindElement(By.Name("password"));
            passwordField.SendKeys("admin");
            var loginButton = _driver.FindElement(By.Name("login"));
            loginButton.Click();
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
    }
}
