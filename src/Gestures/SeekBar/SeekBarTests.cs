using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;


namespace SeekBar
{
    [TestFixture]
    public class SeekBarTests
    {
        private AppiumDriver _driver;
        

        [OneTimeSetUp]
        public void SetUp()
        {
            var serverUri = new Uri("http://192.168.64.23:4723"); // Use the CI server's URL if different
            var appPath = Environment.GetEnvironmentVariable("APK_PATH") ?? "./apk/ApiDemos-debug.apk";

     var androidOptions = new AppiumOptions
  {
      PlatformName = "Android",
      AutomationName = "UIAutomator2",
      DeviceName = "Android Emulator",
      App = appPath
  };

    _driver = new AndroidDriver(serverUri, androidOptions);
    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20); // Increase implicit wait
}

        private void ScrollToText(string text)
        {
            _driver.FindElement(MobileBy.AndroidUIAutomator(
                       "new UiScrollable(new UiSelector().scrollable(true)).scrollIntoView(new UiSelector().text(\"" + text + "\"))"));
            
        }

        [Test]
        public void SeekBarTest()
        {
            var views = _driver.FindElement(MobileBy.AccessibilityId("Views"));
            views.Click();

            ScrollToText("Seek Bar");

            var seekBarOption = _driver.FindElement(MobileBy.AccessibilityId("Seek Bar"));
            seekBarOption.Click();

            // Use the exact coordinates from Appium Inspector
            MoveSeekBarWithInspectorCoordinates(546, 300, 1052, 300);

            var seekBarValueElement = _driver.FindElement(By.Id("io.appium.android.apis:id/progress"));
            var seekBarValueText = seekBarValueElement.Text;
            Assert.That(seekBarValueText, Is.EqualTo("100 from touch=true"), "SeekBar did not move to the expected value.");
        }

        private void MoveSeekBarWithInspectorCoordinates(int startX, int startY, int endX, int endY)
        {
            var finger = new PointerInputDevice(PointerKind.Touch);
            var start = new Point(startX, startY);
            var end = new Point(endX, endY);
            var swipe = new ActionSequence(finger);
            swipe.AddAction(finger.CreatePointerMove(CoordinateOrigin.Viewport, start.X, start.Y, TimeSpan.Zero));
            swipe.AddAction(finger.CreatePointerDown(MouseButton.Left));
            swipe.AddAction(finger.CreatePointerMove(CoordinateOrigin.Viewport, end.X, end.Y, TimeSpan.FromMilliseconds(1000)));
            swipe.AddAction(finger.CreatePointerUp(MouseButton.Left));
            _driver.PerformActions(new List<ActionSequence> { swipe });
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _driver.Quit();
           
        }
    }
}
