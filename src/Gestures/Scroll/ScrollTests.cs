
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Service;


namespace Scroll
{
    [TestFixture]
    public class ScrollAndroidTest
    {
        private AndroidDriver driver;
        private AppiumLocalService appiumLocalService;

         var serverUri = new Uri("http://127.0.0.1:4723/wd/hub"); // Use the CI server's URL if different

     var androidOptions = new AppiumOptions
  {
      PlatformName = "Android",
      AutomationName = "UIAutomator2",
      DeviceName = "Android Emulator",
      App = @"D:\ApiDemos-debug.apk"
  };

    _driver = new AndroidDriver(serverUri, androidOptions);
    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20); // Increase implicit wait
}
        private void ScrollToText(string text)
        {
            driver.FindElement(MobileBy.AndroidUIAutomator(
                "new UiScrollable(new UiSelector().scrollable(true)).scrollIntoView(new UiSelector().text(\"" + text + "\"))"));
        }

        [Test]
        public void ScrollTest()
        {
            var views = driver.FindElement(MobileBy.AccessibilityId("Views"));
            views.Click();

            ScrollToText("Lists");

            var lists = driver.FindElement(MobileBy.AccessibilityId("Lists"));
            Assert.That(lists, Is.Not.Null, "The 'Lists' element was not found after scrolling.");

            lists.Click();

            var elementInList = driver.FindElement(MobileBy.AccessibilityId("10. Single choice list"));
            Assert.That(elementInList, Is.Not.Null, "The expected element in the list was not found.");
        }


        [OneTimeTearDown]
        public void TearDown()
        {
            driver?.Quit();
            appiumLocalService?.Dispose();
        }
    }
}
