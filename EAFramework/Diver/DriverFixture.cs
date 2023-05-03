using EAFramework.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;

namespace EAFramework.Diver;

public class DriverFixture : IDriverFixture, IDisposable
{
    private readonly TestSettings _testSettings;
    public IWebDriver Driver { get; }

    public DriverFixture(TestSettings testSettings)
    {
        _testSettings = testSettings;
        Driver = GetWebDriver();
        Driver.Navigate().GoToUrl(_testSettings.ApplicationUrl);
    }
    // Act like a factory pattern to get the right driver
    private IWebDriver GetWebDriver()
    {
        return _testSettings.BrowserType switch
        {
            BrowserType.Chrome => new ChromeDriver(),
            BrowserType.Firefox => new FirefoxDriver(),
            BrowserType.Safari => new SafariDriver(),
            _ => new ChromeDriver()
        };
    }

    public void Dispose()
    {
        Driver.Quit();
    }
}

public enum BrowserType
{
    Chrome,
    Firefox,
    Safari,
    Edge,
    Chromium
}