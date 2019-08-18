using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SelenuimInitial
{
    [TestFixture]
    public class LoginTest
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        [SetUp]
        public void start()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--no-sandbox");
            _driver = new ChromeDriver(options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        }

        [Test]
        public void LoginToShopTest()
        {
            _driver.Navigate().GoToUrl("http://localhost/litecart/admin/");
            var loginField = _driver.FindElement(By.Name("username"));
            loginField.SendKeys("admin");
            var passwordField = _driver.FindElement(By.Name("password"));
            passwordField.SendKeys("admin");
            var loginButton = _driver.FindElement(By.Name("login"));
            loginButton.Click();
            _wait.Until(el => el.FindElement(By.ClassName("notice success")).Displayed);
        }
        [TearDown]
        public void stop()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}
