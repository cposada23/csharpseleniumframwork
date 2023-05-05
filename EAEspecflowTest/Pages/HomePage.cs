namespace EAEspecflowTest.Pages;

using EAFramework.Diver;
using OpenQA.Selenium;


public interface IHomePage
{
    void ClickProduct();
}

public class HomePage : IHomePage
{
    // Elements
    private IWebElement LnkHome => _driver.FindElement(By.LinkText("Home"));
    private IWebElement LnkPrivacy => _driver.FindElement(By.LinkText("Privacy"));
    private IWebElement LnkProduct => _driver.FindElement(By.LinkText("Product"));

    // Settings
    private readonly IDriverWait _driver;
    public HomePage(IDriverWait driver)
    {
        _driver = driver;
    }
    
    // Actions
    public void ClickProduct() => LnkProduct.Click();
}