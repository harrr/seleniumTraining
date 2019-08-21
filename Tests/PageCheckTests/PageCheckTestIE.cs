using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace SelenuimInitial.Tests
{
    public class PageCheckTestIE : TestBase
    {
        [SetUp]
        public override void Start()
        {
            InternetExplorerOptions options = new InternetExplorerOptions();
            options.RequireWindowFocus = true;
            _driver = new InternetExplorerDriver(options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        }
        [Test]
        public void SortingCheckTest()
        {
            _driver.Navigate().GoToUrl("http://localhost/litecart/admin/");
        }
    }
}
