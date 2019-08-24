using System;
using System.Globalization;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SelenuimInitial.Tests
{
    public class PageCheckTestFF : TestBase
    {
        [SetUp]
        public override void Start()
        {
            FirefoxOptions options = new FirefoxOptions();
            options.UseLegacyImplementation = false;
            _driver = new FirefoxDriver(options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            _driver.Manage().Timeouts().PageLoad = new TimeSpan(0, 2, 0);
        }
        [Test]
        public void PageCheckTest()
        {
            try
            {
                _driver.Navigate().GoToUrl("http://localhost/litecart/");
                IWebElement campainsElement = _driver.FindElement(By.Id("box-campaigns"));
                IWebElement duckElement = campainsElement.FindElement(By.CssSelector("div > ul > li"));
                IWebElement duckLink = duckElement.FindElement(By.CssSelector("a"));
                IWebElement nameMainPageElement = duckElement.FindElement(By.CssSelector("div.name"));
                string nameMainPage = GetTextValue(nameMainPageElement);
                IWebElement ordinaryPriceMainPageElement = duckElement.FindElement(By.CssSelector("a.link > div.price-wrapper > s"));
                string ordinaryPriceMainPageString = GetTextValue(ordinaryPriceMainPageElement);
                int ordinaryPriceMainPage = int.Parse(ordinaryPriceMainPageString.Substring(1), NumberStyles.Any);
                IWebElement campaignPriceMainPageElement = duckElement.FindElement(By.CssSelector("a.link > div.price-wrapper")).FindElement(By.ClassName("campaign-price"));
                string campaignPriceMainPageString = GetTextValue(campaignPriceMainPageElement);
                int campaignPriceMainPage = int.Parse(campaignPriceMainPageString.Substring(1), NumberStyles.Currency);
                string ordinaryPriceColorString = ordinaryPriceMainPageElement.GetCssValue("color");
                CheckGreyColor(ordinaryPriceColorString);
                string isCrossed = ordinaryPriceMainPageElement.GetCssValue("text-decoration");
                Assert.IsTrue(isCrossed.Contains("line-through"));
                string ordinaryPriceFontSizeString = ordinaryPriceMainPageElement.GetCssValue("font-size");
                float ordinaryPriceFontSize = float.Parse(ordinaryPriceFontSizeString.Substring(0, ordinaryPriceFontSizeString.IndexOf('p')), CultureInfo.InvariantCulture);
                string campaignPriceFontSizeString = campaignPriceMainPageElement.GetCssValue("font-size");
                float campaignPriceFontSize = float.Parse(campaignPriceFontSizeString.Substring(0, campaignPriceFontSizeString.IndexOf('p')), CultureInfo.InvariantCulture);
                Assert.Less(ordinaryPriceFontSize, campaignPriceFontSize);
                string campaignPriceColorString = campaignPriceMainPageElement.GetCssValue("color");
                CheckRedColor(campaignPriceColorString);
                string campaignPriceBoldString1 = campaignPriceMainPageElement.GetCssValue("font-weight");
                string ordinaryPriceBoldString = ordinaryPriceMainPageElement.GetCssValue("font-weight");
                Assert.Greater(int.Parse(campaignPriceBoldString1), int.Parse(ordinaryPriceBoldString));
                string campaignPriceBoldString = campaignPriceMainPageElement.GetAttribute("localName");
                Assert.IsTrue(campaignPriceBoldString.Equals("strong"));

                duckLink.Click();
                duckElement = _driver.FindElement(By.Id("box-product"));
                IWebElement nameDuckPageElement = duckElement.FindElement(By.CssSelector("h1"));
                string nameDuckPage = GetTextValue(nameDuckPageElement);
                Assert.True(nameDuckPage.Equals(nameMainPage));
                IWebElement ordinaryPriceDuckPageElement = duckElement.FindElement(By.ClassName("regular-price"));
                string ordinaryPriceDuckPageString = GetTextValue(ordinaryPriceDuckPageElement);
                int ordinaryPriceDuckPage = int.Parse(ordinaryPriceDuckPageString.Substring(1), NumberStyles.AllowCurrencySymbol);
                IWebElement campaignPriceDuckPageElement = duckElement.FindElement(By.ClassName("campaign-price"));
                string campaignPriceDuckPageString = GetTextValue(campaignPriceDuckPageElement);
                int campaignPriceDuckPage = int.Parse(campaignPriceDuckPageString.Substring(1), NumberStyles.AllowCurrencySymbol);
                Assert.AreEqual(ordinaryPriceDuckPage, ordinaryPriceMainPage);
                Assert.AreEqual(campaignPriceDuckPage, campaignPriceMainPage);
                ordinaryPriceColorString = ordinaryPriceDuckPageElement.GetCssValue("color");
                CheckGreyColor(ordinaryPriceColorString);
                isCrossed = ordinaryPriceDuckPageElement.GetCssValue("text-decoration");
                Assert.IsTrue(isCrossed.Contains("line-through"));
                ordinaryPriceFontSizeString = ordinaryPriceDuckPageElement.GetCssValue("font-size");
                ordinaryPriceFontSize = float.Parse(ordinaryPriceFontSizeString.Substring(0, ordinaryPriceFontSizeString.IndexOf('p')), CultureInfo.InvariantCulture);
                campaignPriceFontSizeString = campaignPriceDuckPageElement.GetCssValue("font-size");
                campaignPriceFontSize = float.Parse(campaignPriceFontSizeString.Substring(0, campaignPriceFontSizeString.IndexOf('p')), CultureInfo.InvariantCulture);
                Assert.Less(ordinaryPriceFontSize, campaignPriceFontSize);
                campaignPriceColorString = campaignPriceDuckPageElement.GetCssValue("color");
                CheckRedColor(campaignPriceColorString);
                campaignPriceBoldString1 = campaignPriceDuckPageElement.GetCssValue("font-weight");
                ordinaryPriceBoldString = ordinaryPriceDuckPageElement.GetCssValue("font-weight");
                Assert.Greater(int.Parse(campaignPriceBoldString1), int.Parse(ordinaryPriceBoldString));
                campaignPriceBoldString = campaignPriceDuckPageElement.GetAttribute("localName");
                Assert.IsTrue(campaignPriceBoldString.Equals("strong"));
            }
            catch (Exception ex)
            {
                stop();
                throw ex;
            }
        }

        private string GetTextValue(IWebElement nameElement)
        {
            return nameElement.GetAttribute("innerText");
        }

        private void CheckGreyColor(string colorString)
        {
            colorString = colorString.Replace(" ", string.Empty);
            int startIndex = colorString.IndexOf('(');
            int endIndex = colorString.IndexOf(')');
            var colors = colorString.Substring(startIndex + 1, endIndex - startIndex - 1).Split(',');
            Assert.AreEqual(colors[0], colors[1]);
            Assert.AreEqual(colors[2], colors[1]);
        }
        private void CheckRedColor(string colorString)
        {
            colorString = colorString.Replace(" ", string.Empty);
            colorString = colorString.Replace(")", string.Empty);
            int colorIndex = colorString.IndexOf('(');
            var colors = colorString.Substring(colorIndex + 1).Split(',');
            Assert.NotZero(int.Parse(colors[0]));
            Assert.Zero(int.Parse(colors[1]));
            Assert.Zero(int.Parse(colors[2]));
        }
    }
}
