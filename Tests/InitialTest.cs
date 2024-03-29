﻿using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SelenuimInitial
{
    [TestFixture]
    public class InitialTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        [SetUp]
        public void start()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("no-sandbox");
            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        [Test]
        public void InitialChromeTest()
        {
            driver.Navigate().GoToUrl("https://software-testing.ru/");
            wait.Until(el => el.FindElement(By.ClassName("inputbox-search")));
        }
        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
