using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Interactions;
using System;

namespace Swipe
{
    [TestFixture]
    public class SwipeAndroidTest
    {
        private AndroidDriver _driver;
       

        [OneTimeSetUp]
        public void SetUp()
        {
            var serverUri = new Uri("http://192.168.64.23"); // Use the CI server's URL if different
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

        [Test]
        public void SwipeTest()
        {
            var views = _driver.FindElement(MobileBy.AccessibilityId("Views"));
            views.Click();

            var gallery = _driver.FindElement(MobileBy.AccessibilityId("Gallery"));
            gallery.Click();

            var photos = _driver.FindElement(MobileBy.AccessibilityId("1. Photos"));
            photos.Click();

            var pic1 = _driver.FindElements(By.ClassName("android.widget.ImageView"))[0];

           // Perform swipe action using Actions
            var action = new Actions(_driver);
            var swipe = action.ClickAndHold(pic1)
                              .MoveByOffset(-200, 0)
                              .Release()
                              .Build();
            swipe.Perform();

            // Verify swipe action
            var pic3 = _driver.FindElements(By.ClassName("android.widget.ImageView"))[2];
            Assert.That(pic3, Is.Not.Null, "The third picture was not found after swiping.");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _driver?.Quit();
            
        }
    }
}
