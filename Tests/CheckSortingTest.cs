using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SelenuimInitial
{
    [TestFixture]
    public class CheckSortingTest
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private ReadOnlyCollection<IWebElement> countriesRows;
        IWebElement mainMenuElement;
        [SetUp]
        public void start()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--no-sandbox");
            _driver = new ChromeDriver(options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        }

        [Test]
        public void CountriesChromeTest()
        {
            try
            {
                _driver.Navigate().GoToUrl("http://localhost/litecart/admin");
                Login();
                GetMainElement();
                var menuItems = mainMenuElement.FindElements(By.XPath("//li"));
                IWebElement countriesItem = GetMenuItemByName("Countries", menuItems);
               
                countriesItem.Click();
                GetCountriesRows();
                List<string> countries = new List<string>();
                for (int i = 0; i < countriesRows.Count; i++)
                {
                    var columns = countriesRows[i].FindElements(By.XPath("./td"));
                    countries.Add(columns[4].GetAttribute("innerText"));
                    int zonesNumber = int.Parse(columns[5].GetAttribute("innerText"));
                    if (zonesNumber > 0)
                    {
                        IWebElement link = columns[4].FindElement(By.XPath("./a"));
                        link.Click();
                        CheckZones();
                        GetMainElement();
                        menuItems = mainMenuElement.FindElements(By.XPath("//li"));
                        countriesItem = GetMenuItemByName("Countries", menuItems);
                        countriesItem.Click();
                        GetCountriesRows();
                    }
                }
                CheckSorted(countries);

                GetMainElement();
                menuItems = mainMenuElement.FindElements(By.XPath("//li"));
                IWebElement geoZonesItem = null;
                foreach (IWebElement item in menuItems)
                {
                    IWebElement itemName = item.FindElement(By.XPath("./a/span[@class='name']"));
                    if (itemName.GetAttribute("innerText").Equals("Geo Zones"))
                    {
                        geoZonesItem = item;
                        break;
                    }
                }
                geoZonesItem.Click();
                GetCountriesFromZonesPage();
                for(int i = 0; i < countriesRows.Count; i++)
                {
                    var columns = countriesRows[i].FindElements(By.XPath("./td"));
                    IWebElement link = columns[2].FindElement(By.XPath("./a"));
                    link.Click();
                    CheckZonesForCountry();
                    GetCountriesFromZonesPage();
                }
            }
            catch (Exception ex)
            {
                stop();
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

        private void GetMainElement()
        {
            mainMenuElement = _driver.FindElement(By.Id("box-apps-menu"));
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
        private void CheckZones()
        {
            var zonesTableRows = _driver.FindElements(By.XPath("//table[@id='table-zones']/tbody/tr[not(@class)]"));
            List<string> zones = new List<string>();
            foreach (var row in zonesTableRows)
            {
                var columns = row.FindElements(By.XPath("./td"));
                string zoneName = columns[2].GetAttribute("innerText");
                if(zoneName!=string.Empty)
                    zones.Add(zoneName);
            }
            CheckSorted(zones);
        }

        private void CheckZonesForCountry()
        {
            var zonesTableRows = _driver.FindElements(By.CssSelector("#table-zones > tbody > tr:not(.header)"));
            List<string> zones = new List<string>();
            foreach (var row in zonesTableRows)
            {
                var columns = row.FindElements(By.CssSelector("td"));
                if (columns.Count > 1)
                {
                    var selectedOption = row.FindElement(By.CssSelector("td>select[name*='zone_code']>option[selected]"));
                    zones.Add(selectedOption.GetAttribute("text"));
                }
            }
            CheckSorted(zones);
        }
        private void GetCountriesRows()
        {
            countriesRows = _driver.FindElements(By.XPath("//form[@name='countries_form']/table/tbody/tr[@class='row']"));
        }

        private void GetCountriesFromZonesPage()
        {
            countriesRows = _driver.FindElements(By.XPath("//table[@class='dataTable']//tr[@class='row']"));
        }

        private void CheckSorted(List<string> sequence)
        {
            List<string> sorted = new List<string>();
            sorted.AddRange(sequence.OrderBy(c => c));
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(sorted.SequenceEqual(sequence));
        }
        [TearDown]
        public void stop()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}
