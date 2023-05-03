using OpenQA.Selenium;

namespace EAFramework.Diver;

public interface IDriverWait
{
    IWebElement FindElement(By elementLocator);
    IEnumerable<IWebElement> FindElements(By elementLocator);
}