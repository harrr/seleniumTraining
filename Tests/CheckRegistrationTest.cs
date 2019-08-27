using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SelenuimInitial.Tests
{
    [TestFixture]
    public class CheckRegistrationTest : TestBase
    {
        [SetUp]
        public override void Start()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--no-sandbox");
            _driver = new ChromeDriver(options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        }

        [Test]
        public void CheckRegistrationToShopTest()
        {
            try
            {
                _driver.Navigate().GoToUrl("http://localhost/litecart/");
                IWebElement registrationLink = _driver.FindElement(By.XPath("//form[@name='login_form']//a"));
                registrationLink.Click();
                IWebElement nameField = _driver.FindElement(By.XPath("//form[@name='customer_form']//input[@name='firstname']"));
                string name = "Charlie";
                nameField.SendKeys(name);
                IWebElement surnameField = _driver.FindElement(By.XPath("//form[@name='customer_form']//input[@name='lastname']"));
                string surname = "Richmond";
                surnameField.SendKeys(surname);
                IWebElement addressField = _driver.FindElement(By.XPath("//form[@name='customer_form']//input[@name='address1']"));
                addressField.SendKeys("221 B Baker St");
                IWebElement postCodeField = _driver.FindElement(By.XPath("//form[@name='customer_form']//input[@name='postcode']"));
                postCodeField.SendKeys("22199");
                IWebElement cityField = _driver.FindElement(By.XPath("//form[@name='customer_form']//input[@name='city']"));
                cityField.SendKeys("London");
                IWebElement selectCountryElement = _driver.FindElement(By.Name("country_code"));
                SelectElement countrySelectElement = new SelectElement(selectCountryElement);
                countrySelectElement.SelectByText("United States");
                IWebElement phoneField = _driver.FindElement(By.XPath("//form[@name='customer_form']//input[@name='phone']"));
                phoneField.SendKeys("+1555777333");
                IWebElement emailField = _driver.FindElement(By.XPath("//form[@name='customer_form']//input[@name='email']"));
                string email = $"c.richmond{DateTime.Now.Millisecond}@mail.com";
                emailField.SendKeys(email);
                string password = $"Qwerty{DateTime.Now.Ticks}$";
                IWebElement passwordField = _driver.FindElement(By.XPath("//form[@name='customer_form']//input[@name='password']"));
                passwordField.SendKeys(password);
                //confirmed_password
                IWebElement confirmPasswordField = _driver.FindElement(By.XPath("//form[@name='customer_form']//input[@name='confirmed_password']"));
                confirmPasswordField.SendKeys(password);
                IWebElement createAccountButton = _driver.FindElement(By.Name("create_account"));
                createAccountButton.Click();
                IWebElement selectZoneElement = _driver.FindElement(By.Name("zone_code"));
                SelectElement zoneSelectElement = new SelectElement(selectZoneElement);
                zoneSelectElement.SelectByText("Guam");
                passwordField = _driver.FindElement(By.XPath("//form[@name='customer_form']//input[@name='password']"));
                passwordField.SendKeys(password);
                //confirmed_password
                confirmPasswordField = _driver.FindElement(By.XPath("//form[@name='customer_form']//input[@name='confirmed_password']"));
                confirmPasswordField.SendKeys(password);
                createAccountButton = _driver.FindElement(By.Name("create_account"));
                createAccountButton.Click();
                var accountMenu = _driver.FindElements(By.CssSelector("div.content>ul.list-vertical>li"));
                foreach (var li in accountMenu)
                {
                    IWebElement link = li.FindElement(By.CssSelector("a"));
                    if (link.GetAttribute("innerText").Equals("Logout"))
                    {
                        link.Click();
                        break;
                    }
                }
                IWebElement emailElement = _driver.FindElement(By.Name("email"));
                emailElement.SendKeys(email);
                IWebElement passElement = _driver.FindElement(By.Name("password"));
                passElement.SendKeys(password);
                IWebElement loginElement = _driver.FindElement(By.Name("login"));
                loginElement.Click();
                IWebElement successNote = _driver.FindElement(By.XPath("//div[@id='notices']//div"));
                string successMessage = successNote.GetAttribute("innerText");
                Assert.True(successMessage.Contains(name));
                Assert.True(successMessage.Contains(surname));
            }
            catch (Exception ex)
            {
                stop();
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
