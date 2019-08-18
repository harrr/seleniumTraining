using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SelenuimInitial
{
    [TestFixture]
    public class CheckMenuTest
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
        public void LoginToShopTest()
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
                var mainMenuElement = getMainElement();
                ReadOnlyCollection<IWebElement> mainMenuItems;
                int mainMenuItemsCount = getMainMenuItems(out mainMenuElement, out mainMenuItems);
                for (int i = 0; i < mainMenuItemsCount; i++)
                {
                    mainMenuItems[i].Click();
                    mainMenuElement = getMainElement();
                    var currentSelected = mainMenuElement.FindElement(By.XPath("./li [@class='selected']"));
                    string selectedRef = currentSelected.FindElement(By.XPath("./a")).GetAttribute("href");
                    var childrenListItems = currentSelected.FindElements(By.XPath("./ul/li"));
                    List<string> childrenRefs = childrenListItems.Select(li => li.FindElement(By.XPath("./a")).GetAttribute("href")).ToList();
                    foreach(string childRef in childrenRefs)
                    {
                        IWebElement child = getMainElement().FindElement(By.XPath($"//a[@href = '{childRef}']"));
                        IWebElement h1 = _driver.FindElement(By.XPath("//h1"));
                        if (h1 == null | !h1.Displayed)
                            throw new Exception("H1 not found");
                        child.Click();
                    }
                    getMainMenuItems(out mainMenuElement, out mainMenuItems);
                }
            }
            catch (Exception ex)
            {
                stop();
                throw ex;
            }
        }

        private int getMainMenuItems(out IWebElement mainMenuElement, out ReadOnlyCollection<IWebElement> mainMenuItems)
        {
            mainMenuElement = getMainElement();
            mainMenuItems = mainMenuElement.FindElements(By.XPath("./li"));
            return mainMenuItems.Count;
        }

        private IWebElement getMainElement()
        {
            return _driver.FindElement(By.Id("box-apps-menu"));
        }
        private void ProcessItem(IWebElement element)
        {
            var children = GetChildrenItems(element);
            for (int i = 0; i < children.Count; i++)
            {
                var cs = GetChildrenItems(element);
                ProcessItem(cs[i]);
            }
            element.Click();
        }
        private ReadOnlyCollection<IWebElement> GetChildrenItems(IWebElement element)
        {
            return element.FindElements(By.XPath("//li")); 
        }

        [TearDown]
        public void stop()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}
