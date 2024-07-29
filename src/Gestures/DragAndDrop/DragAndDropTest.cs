using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.MultiTouch;
using System;
using System.Collections.Generic;

namespace DragAndDrop
{
    [TestFixture]
    public class DragDropTest
    {
        private AndroidDriver _driver;

        [OneTimeSetUp]
        public void SetUp()
        {
            var serverUri = new Uri(Environment.GetEnvironmentVariable("APPIUM_SERVER_URI") ?? "http://192.168.64.24:4723");
            var appPath = Environment.GetEnvironmentVariable("APK_PATH") ?? "./apk/ApiDemos-debug.apk";
            var androidOptions = new AppiumOptions
            {
                PlatformName = "Android",
                AutomationName = "UIAutomator2",
                DeviceName = "pixel",
                App = appPath,
            };

            // Ensure to use a version where commandTimeout is correctly handled
            _driver = new AndroidDriver(serverUri, androidOptions, TimeSpan.FromMinutes(5)); 
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
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
        }
    }
}
