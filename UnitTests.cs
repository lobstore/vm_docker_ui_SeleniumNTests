using System;
namespace vm_docker_ui_NTests
{
    public class Tests
    {
        private ChromeOptions _chromeOptions;
        private string remoteUrlDriverChrome = "http://localhost:4444/";
        private string homepage = "http://host.docker.internal:5000";
        private IWebDriver _webdriver;
        private WebDriverWait _wait;

        [SetUp]
        public void Setup()
        {
            _chromeOptions = new ChromeOptions();
            _webdriver = new RemoteWebDriver(new Uri(remoteUrlDriverChrome), _chromeOptions);
            _webdriver.Manage().Window.Maximize();
            _wait = new WebDriverWait(_webdriver, TimeSpan.FromSeconds(10));
            _webdriver.Navigate().GoToUrl(homepage);
        }

        [TestCase("example@gmail.com", "test1", "test1234")]
        [TestCase("example@yandex.ru", "test2", "test1234")]
        [TestCase("example@mail.ru", "test3", "test1234")]
        public void UserRegistrationSmokeTests(string email, string name, string password)
        {
            ClickElementByXPath("//*[@id=\"navbarMenuHeroA\"]/div/a[3]");
            SendKeysToElementByXPath("/html/body/section/div[2]/div/div/div/form/div[1]/div/input", email);
            SendKeysToElementByXPath("/html/body/section/div[2]/div/div/div/form/div[2]/div/input", name);
            SendKeysToElementByXPath("/html/body/section/div[2]/div/div/div/form/div[3]/div/input", password);
            ClickElementByXPath("/html/body/section/div[2]/div/div/div/form/button");

            _webdriver.Navigate().GoToUrl(homepage);
            ClickElementByXPath("//*[@id=\"navbarMenuHeroA\"]/div/a[2]");
            SendKeysToElementByXPath("/html/body/section/div[2]/div/div/div/form/div[1]/div/input", email);
            SendKeysToElementByXPath("/html/body/section/div[2]/div/div/div/form/div[2]/div/input", password);
            ClickElementByXPath("/html/body/section/div[2]/div/div/div/form/button");

            _webdriver.Navigate().GoToUrl(homepage);
            ClickElementByXPath("//*[@id=\"navbarMenuHeroA\"]/div/a[2]");
            string welcomeText = GetElementTextByXPath("/html/body/section/div[2]/div/h1");
            Assert.That(welcomeText == $"Welcome, {name}!", "The user under the correct data was not created");
        }

        [TestCase("", "test4", "")]
        [TestCase("example1@gmail.com", "test5", "")]
        [TestCase("example1", "test6", "test1234")]
        [TestCase("example1@", "test7", "test1234")]
        [TestCase("example1@gmail", "test8", "test1234")]
        public void UserRegistrationNegativeTests(string email, string name, string password)
        {
            ClickElementByXPath("//*[@id=\"navbarMenuHeroA\"]/div/a[3]");
            SendKeysToElementByXPath("/html/body/section/div[2]/div/div/div/form/div[1]/div/input", email);
            SendKeysToElementByXPath("/html/body/section/div[2]/div/div/div/form/div[2]/div/input", name);
            SendKeysToElementByXPath("/html/body/section/div[2]/div/div/div/form/div[3]/div/input", password);
            ClickElementByXPath("/html/body/section/div[2]/div/div/div/form/button");

            _webdriver.Navigate().GoToUrl(homepage);
            ClickElementByXPath("//*[@id=\"navbarMenuHeroA\"]/div/a[2]");
            SendKeysToElementByXPath("/html/body/section/div[2]/div/div/div/form/div[1]/div/input", email);
            SendKeysToElementByXPath("/html/body/section/div[2]/div/div/div/form/div[2]/div/input", password);
            ClickElementByXPath("/html/body/section/div[2]/div/div/div/form/button");

            _webdriver.Navigate().GoToUrl(homepage);
            ClickElementByXPath("//*[@id=\"navbarMenuHeroA\"]/div/a[2]");
            Assert.That(!_webdriver.PageSource.Contains("Profile") && !_webdriver.PageSource.Contains($"Welcome, {name}!"), "The user was created under incorrect data");
        }

        [TestCase(360, 680)]
        [TestCase(680, 360)]
        [TestCase(768, 1024)]
        [TestCase(1024, 768)]
        [TestCase(1366, 768)]
        [TestCase(768, 1366)]
        [TestCase(1920, 1080)]
        public void ChangeWindowSizeHomePageElementsVisibilityTests(int width, int height)
        {
            _webdriver.Manage().Window.Size = new System.Drawing.Size(width, height);
            string bugtext = $"is not visible at {width}x{height}px";

            IWebElement homeButton = FindElementByXPath("//*[@id=\"navbarMenuHeroA\"]/div/a[1]");
            IWebElement loginButton = FindElementByXPath("//*[@id=\"navbarMenuHeroA\"]/div/a[2]");
            IWebElement registrationButton = FindElementByXPath("//*[@id=\"navbarMenuHeroA\"]/div/a[3]");
            IWebElement welcomeText = FindElementByXPath("/html/body/section/div[2]/div/h1");

            Assert.IsTrue(homeButton.Displayed, $"Home button {bugtext}");
            Assert.IsTrue(loginButton.Displayed, $"Login button {bugtext}");
            Assert.IsTrue(registrationButton.Displayed, $"Registration button {bugtext}");
            Assert.IsTrue(welcomeText.Displayed, $"Welcome text {bugtext}");
        }

        [TestCase("example2@gmail.com", "test9", "test1234")]
        public void RememberMeFeatureTest(string email, string name, string password)
        {
            ClickElementByXPath("//*[@id=\"navbarMenuHeroA\"]/div/a[2]");
            SendKeysToElementByXPath("/html/body/section/div[2]/div/div/div/form/div[1]/div/input", email);
            SendKeysToElementByXPath("/html/body/section/div[2]/div/div/div/form/div[2]/div/input", password);
            ClickElementByXPath("/html/body/section/div[2]/div/div/div/form/button");

            _webdriver.Quit();
            Setup();

            _webdriver.Navigate().GoToUrl(homepage);
            ClickElementByXPath("//*[@id=\"navbarMenuHeroA\"]/div/a[2]");
            string welcomeText = GetElementTextByXPath("/html/body/section/div[2]/div/h1");
            Assert.That(welcomeText == $"Welcome, {name}!", "The \"Remember me\" function does not work after closing the browser");
        }

        [TearDown]
        public void TearDown()
        {
            _webdriver.Quit();
        }

        private void ClickElementByXPath(string xPath)
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xPath))).Click();
        }

        private void SendKeysToElementByXPath(string xPath, string text)
        {
            _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath))).SendKeys(text);
        }

        private string GetElementTextByXPath(string xPath)
        {
            return _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath))).Text;
        }

        private IWebElement FindElementByXPath(string xPath)
        {
            return _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath)));
        }
    }
}
