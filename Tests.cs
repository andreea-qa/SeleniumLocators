using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumLocators
{
    public class Tests
    {
        private static IWebDriver driver;
        private static readonly string gridURL = "@hub.lambdatest.com/wd/hub";
        private static readonly string LT_USERNAME = Environment.GetEnvironmentVariable("LT_USERNAME"); 
        private static readonly string LT_ACCESS_KEY = Environment.GetEnvironmentVariable("LT_ACCESS_KEY");
        private readonly string loginURL = "https://ecommerce-playground.lambdatest.io/index.php?route=account/login";
        private readonly string homePageURL = "https://ecommerce-playground.lambdatest.io/index.php";

        [SetUp]
        public void Setup()
        {
            DriverOptions capabilities = new SafariOptions();
            capabilities.BrowserVersion = "16.0";
            Dictionary<string, object> ltOptions = new Dictionary<string, object>();
            ltOptions.Add("username", LT_USERNAME);
            ltOptions.Add("accessKey", LT_ACCESS_KEY);
            ltOptions.Add("platformName", "MacOS Ventura");
            ltOptions.Add("build", "[DEMO] E-commerce"); ltOptions.Add("w3c", true);
            ltOptions.Add("project", "LambdaTest Playground");
            ltOptions.Add("plugin", "c#-c#");
            capabilities.AddAdditionalOption("LT:Options", ltOptions);
            driver = new RemoteWebDriver(new Uri($"https://{LT_USERNAME}:{LT_ACCESS_KEY}{gridURL}"), capabilities);
        }

        [Test]
        public void ValidateSignIn()
        {
            driver.Navigate().GoToUrl(loginURL);
            driver.FindElement(By.Name("email")).SendKeys("andreea@getnada.com");
            driver.FindElement(By.Id("input-password")).SendKeys("test");
            driver.FindElement(By.CssSelector("#content > div > div:nth-child(2) > div > div > form > input")).Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//h2[text()='My Account']")));
        }

        [Test]
        public void ValidateNumberOfProducts()
        {
            int expectedNumber = 8; 
            driver.Navigate().GoToUrl(homePageURL);
            driver.FindElement(By.Name("search")).SendKeys("htc");
            driver.FindElement(By.XPath("//button[text()='Search']")).Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//h1[text()='Search - htc']")));
            var results = driver.FindElements(By.XPath("//a[text()='HTC Touch HD']"));
            Assert.That(results, Has.Count.EqualTo(expectedNumber));
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}