using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace ShoppingListPatterns
{
    public class CardPage : Page
    {
        public CardPage(IWebDriver driver) : base(driver) => PageFactory.InitElements(driver, this);

        [FindsBy(How = How.Name, Using = "remove_cart_item")]
        IWebElement removeButton;

        public void RemoveDuck()
        {
            base.wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("remove_cart_item")));
            removeButton.Click();
        }
    }
}
