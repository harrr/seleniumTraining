using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace ShoppingListPatterns
{
    public class DuckPage : Page
    {
        public DuckPage(IWebDriver driver) : base(driver) => PageFactory.InitElements(driver, this);

        [FindsBy(How = How.Name, Using = "add_cart_product")]
        IWebElement addToCart;

        public void AddToCart()
        {
            base.wait.Until(ExpectedConditions.ElementToBeClickable(addToCart));
            addToCart.Click();
        }
    }
}
