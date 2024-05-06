
using System.Drawing;

namespace vm_docker_ui_NTests
{
    
    public class Tests
    {
        ChromeOptions _chromeOptions;
        string remote_url_driver_chrome = "http://localhost:4444/";
        string homepage = "http://host.docker.internal:5000";
        IWebDriver _webdriver;
        [SetUp]
        public void Setup()
        {
            _chromeOptions = new ChromeOptions();
            _webdriver = new RemoteWebDriver(new Uri(remote_url_driver_chrome), _chromeOptions);
            _webdriver.Manage().Window.Maximize();
            _webdriver.Navigate().GoToUrl(homepage);
        }
        
        [TestCase("example@gmail.com", "test1", "test1234")]
        [TestCase("example@yandex.ru", "test2", "test1234")]
        [TestCase("example@mail.ru", "test3", "test1234")]
        public void UserRegistrationSmokeTests(string email, string name, string password)
        {

            _webdriver.FindElement(By.XPath("//*[@id=\"navbarMenuHeroA\"]/div/a[3]")).Click();
            _webdriver.FindElement(By.XPath("/html/body/section/div[2]/div/div/div/form/div[1]/div/input")).SendKeys(email);
            _webdriver.FindElement(By.XPath("/html/body/section/div[2]/div/div/div/form/div[2]/div/input")).SendKeys(name);
            _webdriver.FindElement(By.XPath("/html/body/section/div[2]/div/div/div/form/div[3]/div/input")).SendKeys(password);
            _webdriver.FindElement(By.XPath("/html/body/section/div[2]/div/div/div/form/button")).Click();
            Thread.Sleep(2000);
            _webdriver.Navigate().GoToUrl(homepage);
            _webdriver.FindElement(By.XPath("//*[@id=\"navbarMenuHeroA\"]/div/a[2]")).Click();
            _webdriver.FindElement(By.XPath("/html/body/section/div[2]/div/div/div/form/div[1]/div/input")).SendKeys(email);
            _webdriver.FindElement(By.XPath("/html/body/section/div[2]/div/div/div/form/div[2]/div/input")).SendKeys(password);
            _webdriver.FindElement(By.XPath("/html/body/section/div[2]/div/div/div/form/button")).Click();
            Thread.Sleep(2000);
            _webdriver.Navigate().GoToUrl(homepage);
            _webdriver.FindElement(By.XPath("//*[@id=\"navbarMenuHeroA\"]/div/a[2]")).Click();
            Assert.That(_webdriver.FindElement(By.XPath("/html/body/section/div[2]/div/h1")).Text == $"Welcome, {name}!", "The user under the correct data was not created");
        }
        [TestCase("", "test4", "")]
        [TestCase("example1@gmail.com", "test5", "")]
        [TestCase("example1", "test6", "test1234")]
        [TestCase("example1@", "test7", "test1234")]
        [TestCase("example1@gmail", "test8", "test1234")]
        public void UserRegistrationNegativeTests(string email, string name, string password)
        {

            _webdriver.FindElement(By.XPath("//*[@id=\"navbarMenuHeroA\"]/div/a[3]")).Click();
            _webdriver.FindElement(By.XPath("/html/body/section/div[2]/div/div/div/form/div[1]/div/input")).SendKeys(email);
            _webdriver.FindElement(By.XPath("/html/body/section/div[2]/div/div/div/form/div[2]/div/input")).SendKeys(name);
            _webdriver.FindElement(By.XPath("/html/body/section/div[2]/div/div/div/form/div[3]/div/input")).SendKeys(password);
            _webdriver.FindElement(By.XPath("/html/body/section/div[2]/div/div/div/form/button")).Click();
            Thread.Sleep(2000);
            _webdriver.Navigate().GoToUrl(homepage);
            _webdriver.FindElement(By.XPath("//*[@id=\"navbarMenuHeroA\"]/div/a[2]")).Click();
            _webdriver.FindElement(By.XPath("/html/body/section/div[2]/div/div/div/form/div[1]/div/input")).SendKeys(email);
            _webdriver.FindElement(By.XPath("/html/body/section/div[2]/div/div/div/form/div[2]/div/input")).SendKeys(password);
            _webdriver.FindElement(By.XPath("/html/body/section/div[2]/div/div/div/form/button")).Click();
            Thread.Sleep(2000);
            _webdriver.Navigate().GoToUrl(homepage);
            _webdriver.FindElement(By.XPath("//*[@id=\"navbarMenuHeroA\"]/div/a[2]")).Click();
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
            _webdriver.Manage().Window.Size = new Size(width, height);
            string bugtext = $"is not visible at {width}x{height}px";
            IWebElement homebutton = _webdriver.FindElement(By.XPath("//*[@id=\"navbarMenuHeroA\"]/div/a[1]"));
            IWebElement loginbutton = _webdriver.FindElement(By.XPath("//*[@id=\"navbarMenuHeroA\"]/div/a[2]"));
            IWebElement registrationbutton = _webdriver.FindElement(By.XPath("//*[@id=\"navbarMenuHeroA\"]/div/a[3]"));
            IWebElement welcometext = _webdriver.FindElement(By.XPath("/html/body/section/div[2]/div/h1"));
            try
            {
                Assert.IsTrue(homebutton.Displayed, $"Home button {bugtext}");
            }
            catch (AssertionException)
            { }
            try
            {
                Assert.IsTrue(loginbutton.Displayed, $"Login button {bugtext}");
            }
            catch (AssertionException)
            { }
            try
            {
                Assert.IsTrue(registrationbutton.Displayed, $"Registration button {bugtext}");
            }
            catch (AssertionException)
            { }
            try
            {
                Assert.IsTrue(welcometext.Displayed, $"Welcome text {bugtext}");
            }
            catch (AssertionException)
            { }


            Thread.Sleep(2000);
        }

        [TestCase("example2@gmail.com", "test9", "test1234")]
        public void RememberMeFeatureTest(string email, string name, string password)
        {
            
            _webdriver.FindElement(By.XPath("//*[@id=\"navbarMenuHeroA\"]/div/a[2]")).Click();
            _webdriver.FindElement(By.XPath("/html/body/section/div[2]/div/div/div/form/div[1]/div/input")).SendKeys(email);
            _webdriver.FindElement(By.XPath("/html/body/section/div[2]/div/div/div/form/div[2]/div/input")).SendKeys(password);
            _webdriver.FindElement(By.XPath("/html/body/section/div[2]/div/div/div/form/button")).Click();
            _webdriver.Quit();
            _webdriver.Navigate().GoToUrl(homepage);
            _webdriver.FindElement(By.XPath("//*[@id=\"navbarMenuHeroA\"]/div/a[2]")).Click();
            Assert.That(_webdriver.FindElement(By.XPath("/html/body/section/div[2]/div/h1")).Text == $"Welcome, {name}!", "The \"Remember me\" function does not work after closing the browser");
        }
        
        [TearDown]
        public void TearDown()
        {
            _webdriver.Quit();
        }
    }
}