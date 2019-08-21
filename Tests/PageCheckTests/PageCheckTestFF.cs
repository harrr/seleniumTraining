using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
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
        }
        [Test]
        public void SortingCheckTest()
        {
            _driver.Navigate().GoToUrl("http://localhost/litecart/admin/");
        }
    }
}
