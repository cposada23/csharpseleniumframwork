using OpenQA.Selenium;

namespace EAFramework.Diver;

public interface IDriverFixture
{
    IWebDriver Driver { get; }
}