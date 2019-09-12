using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace ShoppingListPatterns
{
    public class LiteCardApp
    {
        IWebDriver driver;
        CatalogPage catalogPage;
        DuckPage duckPage;
        CardPage cardPage;
        int numberOfDucksinCart = 0;
        public LiteCardApp()
        {
            var options = new ChromeOptions();
            
            options.AddArguments("--no-sandbox");
            options.AddArguments("--start-maximized");
            driver = new ChromeDriver(options);
            catalogPage = new CatalogPage(driver);
            duckPage = new DuckPage(driver);
            cardPage = new CardPage(driver);
        }
        public void OpenCatalog()
        {
            driver.Navigate().GoToUrl("http://localhost/litecart/");
        }

        public void AddDucks(int numberOfDucks)
        {
            for(int i = 0; i < numberOfDucks; i++)
            {
                AddDuck(i);
            }
            numberOfDucksinCart = numberOfDucks;
        }

        void AddDuck(int i)
        {
            catalogPage.AddDuck(i);
            duckPage.AddToCart();
            driver.Navigate().Back();
            catalogPage.WaitQuantityToChange(i);
        }

        public void DeleteDucks(int numberOfDucks)
        {
            if (numberOfDucks > numberOfDucksinCart)
                throw new ArgumentOutOfRangeException("Not enough ducks");
            for(int i = 0; i < numberOfDucks; i++)
            {
                cardPage.RemoveDuck();
            }
        }

        public void OpenCart()
        {
            catalogPage.OpenCart();
        }
        
        public void Quit()
        {
            driver.Quit();
            driver = null;
        }

    }
}
