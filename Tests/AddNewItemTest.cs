using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;

namespace SelenuimInitial.Tests
{
    [TestFixture]
    public class AddNewItemTest : TestBase
    {
        private IJavaScriptExecutor _js;
        private ReadOnlyCollection<IWebElement> countriesRows;
        IWebElement mainMenuElement;
        [SetUp]
        public override void Start()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--no-sandbox");
            _driver = new ChromeDriver(options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            _js = (IJavaScriptExecutor)_driver;
        }
        [Test]
        public void RunAddNewItemTest()
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
                menuItems[1].Click();
                IWebElement addItemButton = _driver.FindElement(By.XPath("//td[@id='content']//div//a[2]"));
                addItemButton.Click();
                IWebElement categoryAllElement = _driver.FindElement(By.XPath("//input[@name='status' and @value='1']"));
                categoryAllElement.Click();
                IWebElement categoryRubberElement = _driver.FindElement(By.XPath("//input[@name='status' and @value='0']"));
                categoryRubberElement.Click();
                IWebElement statusRadio = _driver.FindElement(By.XPath("//input[@name='status' and @value='1']"));
                statusRadio.Click();
                IWebElement nameElement = _driver.FindElement(By.Name("name[en]"));
                string code = DateTime.Now.Millisecond.ToString();
                string name = $"Tower Duck {code}";
                nameElement.SendKeys(name);
                IWebElement codeElement = _driver.FindElement(By.Name("code"));
                codeElement.SendKeys(code);
                IWebElement allGenderChBox = _driver.FindElement(By.XPath("//div[@class='input-wrapper']/table/tbody/tr[4]/td[1]/input"));
                allGenderChBox.Click();
                IWebElement quantityElement = _driver.FindElement(By.Name("quantity"));
                quantityElement.Clear();
                quantityElement.SendKeys("20");
                IWebElement quantityUnitElement = _driver.FindElement(By.Name("quantity_unit_id"));
                SelectElement quantityUnitSelectElement = new SelectElement(quantityUnitElement);
                quantityUnitSelectElement.SelectByText("pcs");
                IWebElement soldOutElement = _driver.FindElement(By.Name("sold_out_status_id"));
                SelectElement soldOutSelectElement = new SelectElement(soldOutElement);
                quantityUnitSelectElement.SelectByIndex(0);
                IWebElement fileDialog = _driver.FindElement(By.Name("new_images[]"));
                string appPath = AppDomain.CurrentDomain.BaseDirectory + "tower.jpg";
                attachFile(fileDialog, appPath);
                IWebElement dateFromElement = _driver.FindElement(By.Name("date_valid_from"));
                dateFromElement.SendKeys(DateTime.Today.ToShortDateString());
                IWebElement dateToElement = _driver.FindElement(By.Name("date_valid_to"));
                dateToElement.SendKeys(DateTime.Today.AddYears(10).ToShortDateString());
                //information
                findMenuItemByTextAndClick("Information");
                IWebElement manufacturerSelectionElement = _driver.FindElement(By.Name("manufacturer_id"));
                SelectElement manufacturerSelectElement = new SelectElement(manufacturerSelectionElement);
                manufacturerSelectElement.SelectByIndex(1);
                IWebElement keywordsElement = _driver.FindElement(By.Name("keywords"));
                keywordsElement.SendKeys("rubber duck, duck");
                IWebElement shortDescriptionElement = _driver.FindElement(By.Name("short_description[en]"));
                shortDescriptionElement.SendKeys("Tower rubber duck");
                IWebElement descriptionElement = _driver.FindElement(By.Name("description[en]"));
                descriptionElement.SendKeys("Tower rubber duck");
                IWebElement headTitleElement = _driver.FindElement(By.Name("head_title[en]"));
                headTitleElement.SendKeys("Tower rubber duck");
                //prices
                findMenuItemByTextAndClick("Prices");
                IWebElement purchasePriceElement = _driver.FindElement(By.Name("purchase_price"));
                purchasePriceElement.SendKeys("12");

                IWebElement purchasePriceCurrencyElement = _driver.FindElement(By.Name("purchase_price_currency_code"));
                SelectElement purchasePriceCurrencySelectElement = new SelectElement(purchasePriceCurrencyElement);
                purchasePriceCurrencySelectElement.SelectByValue("USD");

                IWebElement priceElement = _driver.FindElement(By.Name("prices[USD]"));
                priceElement.SendKeys("12");
                Save();
                var ducks = _driver.FindElements(By.XPath("//tr[@class='row']//td//a[not(@title)]"));
                bool found = false;
                foreach (var duck in ducks)
                {
                    if(duck.GetAttribute("innerText").Equals(name))
                    {
                        found = true;
                        break;
                    }
                }
                Assert.True(found);
            }
            catch (Exception ex)
            {
                stop();
                throw ex;
            }
        }
        private void show(IWebElement element)
        {
            String script = "arguments[0].style.opacity=1;"
                            + "arguments[0].style['transform']='translate(0px, 0px) scale(1)';"
                            + "arguments[0].style['MozTransform']='translate(0px, 0px) scale(1)';"
                            + "arguments[0].style['WebkitTransform']='translate(0px, 0px) scale(1)';"
                            + "arguments[0].style['msTransform']='translate(0px, 0px) scale(1)';"
                            + "arguments[0].style['OTransform']='translate(0px, 0px) scale(1)';"
                            + "return true;";
            _js.ExecuteScript(script, element);
        }

        private void attachFile(IWebElement element, String file)
        {
            show(element);
            element.SendKeys(file);
        }

        private void findMenuItemByTextAndClick(string itemText)
        {
            ReadOnlyCollection<IWebElement> menuElements = _driver.FindElements(By.CssSelector("ul.index>li>a"));
            foreach (IWebElement item in menuElements)
            {
                if (item.GetAttribute("innerText").Equals(itemText))
                {
                    item.Click();
                }
            }
        }
        private void Save()
        {
            IWebElement saveButton = _driver.FindElement(By.Name("save"));
            saveButton.Click();
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
        [TearDown]
        public void stop()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}
