using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace ShoppingListPatterns
{
    public class CatalogPage : Page
    {
        public CatalogPage(IWebDriver driver) : base(driver) => PageFactory.InitElements(driver, this);
        private const string ducksSelector = "//div[@id = 'box-most-popular']//div[@class = 'content']//ul//li//a[@class='link' and not(@title = 'Yellow Duck')]";

        [FindsBy(How = How.XPath, Using = ducksSelector)]
        IList<IWebElement> ducksToAdd;

        [FindsBy(How = How.CssSelector, Using = "span.quantity")]
        IWebElement quantity;

        [FindsBy(How = How.Id, Using = "cart")]
        IWebElement cart;

        public void AddDuck(int i)
        {
            base.wait.Until(ExpectedConditions.ElementToBeClickable(ducksToAdd[i]));
            ducksToAdd[i].Click();
        }

        public void WaitQuantityToChange(int i)
        {
            base.wait.Until(ExpectedConditions.ElementToBeClickable(quantity));
            base.wait.Until(ExpectedConditions.TextToBePresentInElement(quantity, (i + 1).ToString()));
        }

        public void OpenCart()
        {
            cart.Click();
        }
    }
}
