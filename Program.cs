using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

class DynamicElementHandling
{
    static void Main(string[] args)
    {
        var options = new ChromeOptions();
        IWebDriver driver = new ChromeDriver(options);

        String nodeURL = "http://localhost:8085";
 
                

        try
        {
            driver.Navigate().GoToUrl("https://salesforce.berounds.com");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(40));

            try
            {
                bool isPageReady = (bool)((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState == 'complete'");
                Console.WriteLine("Page load status: " + isPageReady);

                // Example: Trying multiple locators in sequence (XPath, CSS, ID)
                IWebElement bookCallButton = TryFindElement(driver, wait, new[]
                {
                    By.XPath("(//button[@type='button'])[3]"),
                    By.CssSelector(".btn-book-call"),
                    By.Id("bookCallBtn")
                });

                if (bookCallButton != null)
                {
                    Console.WriteLine("Found 'Book a Call' button.");
                    bookCallButton.Click();
                    Console.WriteLine("Test Passed - Book a Call button was clicked.");
                }
                else
                {
                    Console.WriteLine("Test Failed - Could not find 'Book a Call' button.");
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine("Test Failed: 'Book a Call' button not found within the specified timeout.");
                Console.WriteLine("Exception: " + ex.Message);
            }

            System.Threading.Thread.Sleep(30000); // Keep the browser open for 30 seconds
        }
        finally
        {
            driver.Quit();
        }
    }

    // This method tries to find the element using different locators
    static IWebElement TryFindElement(IWebDriver driver, WebDriverWait wait, By[] locators)
    {
        foreach (var locator in locators)
        {
            try
            {
                var element = wait.Until(ExpectedConditions.ElementToBeClickable(locator));
                if (element != null)
                {
                    return element;
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine($"Element not found using locator: {locator}");
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"Element not clickable with locator: {locator}");
            }
        }
        return null;
    }
}
