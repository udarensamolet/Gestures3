using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Service.Options;
using OpenQA.Selenium.Appium.Android.UiAutomator;

// Correct namespace and class definition
namespace Scroll
{
    [TestFixture]
    public class ScrollAndroidTest
    {
        private AndroidDriver _driver;

       
        [OneTimeSetUp]
        public void SetUp()
        {
            // Define server URL and Appium options
            var serverUri = new Uri("http://192.168.64.24:4723");
            var appPath = Environment.GetEnvironmentVariable("APK_PATH") ?? "./apk/ApiDemos-debug.apk";
            var androidOptions = new AppiumOptions
            {
                PlatformName = "Android",
                AutomationName = "UIAutomator2",
                DeviceName = "Android Emulator",
                App = appPath // Ensure this path is valid in your CI environment
            };

            _driver = new AndroidDriver(serverUri, androidOptions);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20); // Increase implicit wait
        }

        private void ScrollToText(string text)
        {
            _driver.FindElement(MobileBy.AndroidUIAutomator(
                $"new UiScrollable(new UiSelector().scrollable(true)).scrollIntoView(new UiSelector().text(\"{text}\"))"));
        }

        [Test]
        public void ScrollTest()
        {
            var views = _driver.FindElement(MobileBy.AccessibilityId("Views"));
            views.Click();

            ScrollToText("Lists");

            var lists = _driver.FindElement(MobileBy.AccessibilityId("Lists"));
            Assert.That(lists, Is.Not.Null, "The 'Lists' element was not found after scrolling.");

            lists.Click();

            var elementInList = _driver.FindElement(MobileBy.AccessibilityId("10. Single choice list"));
            Assert.That(elementInList, Is.Not.Null, "The expected element in the list was not found.");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _driver?.Quit();
        }
    }
}
