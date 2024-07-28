using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.MultiTouch;
using System;
using OpenQA.Selenium.Interactions;
using System.Drawing;

namespace DragAndDrop
{
    [TestFixture]
    public class DragDropTest
    {
        private AndroidDriver _driver;

        [OneTimeSetUp]
        public void SetUp()
        {
            var serverUri = new Uri(Environment.GetEnvironmentVariable("APPIUM_SERVER_URI") ?? "127.0.0.1:4723");
            var appPath = Environment.GetEnvironmentVariable("APK_PATH") ?? "./apk/ApiDemos-debug.apk";
            var androidOptions = new AppiumOptions
            {
                PlatformName = "Android",
                AutomationName = "UIAutomator2",
                DeviceName = "pixel",
                App = appPath
            };

            var commandTimeout = TimeSpan.FromMinutes(3); // or any other appropriate duration
            _driver = new AndroidDriver(serverUri, androidOptions);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20); // Increase implicit wait
        }

        [Test]
        public void DragAndDropTest()
        {
            var views = _driver.FindElement(MobileBy.AccessibilityId("Views"));
            views.Click();

            var dragDrop = _driver.FindElement(MobileBy.AccessibilityId("Drag and Drop"));
            dragDrop.Click();

            var drag = _driver.FindElement(By.Id("drag_dot_1"));
            var drop = _driver.FindElement(By.Id("drag_dot_2"));

            // Perform the drag and drop action using JavaScript ExecutScript (mobile: dragGesture)
            var scriptArgs = new Dictionary<string, object>
            {
                { "elementId", drag.Id },
                { "endX", drop.Location.X + (drop.Size.Width / 2) },
                { "endY", drop.Location.Y + (drop.Size.Height / 2) },
                { "speed", 2500 } // Optional speed parameter
            };

            _driver.ExecuteScript("mobile: dragGesture", scriptArgs);

            // Assertion: Verify the text indicating a successful drop
            var dropSuccessMessage = _driver.FindElement(By.Id("drag_result_text"));
            Assert.That(dropSuccessMessage.Text, Is.EqualTo("Dropped!"), "The drag and drop action did not complete successfully.");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _driver?.Quit();
            // Removed the _appiumLocalService reference as it is not used in this context
        }
    }
}
