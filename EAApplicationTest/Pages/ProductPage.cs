using EAFramework.Diver;
using EAFramework.Extensions;
using OpenQA.Selenium;
using TestProject1.Models;

namespace TestProject1.Pages;

public interface IProductPage
{
    void PerformActionForProduct(string productName, TableActions action);
    void ClickCreate();
    void CreateProduct(Product product);
    void GoToProductDetails(string name);
    void GoToEditDetails(string name);
    void GoDeleteProduct(string name);
}

public class ProductPage : IProductPage
{
    private IWebElement InputName => _driver.FindElement(By.Id("Name"));
    private IWebElement InputDescription => _driver.FindElement(By.Id("Description"));
    private IWebElement InputPrice => _driver.FindElement(By.Id("Price"));
    private IWebElement SelectProductType => _driver.FindElement(By.Id("ProductType"));
    private IWebElement LnkBackToList => _driver.FindElement(By.LinkText("Back to List"));
    private IWebElement LnkCreate => _driver.FindElement(By.LinkText("Create"));
    private IWebElement BtnCreate => _driver.FindElement(By.Id("Create"));

    public void PerformActionForProduct(string productName, TableActions action)
    {
        var xpath = $"//table//tbody//td[contains(text(), '{productName}')]/..//td/a[contains(text(), '{action.ToString()}')]";
        _driver.FindElement(By.XPath(xpath)).Click();
    }
    
    private readonly IDriverWait _driver;
    public ProductPage(IDriverWait driver)
    {
        _driver = driver;
    }

    public void ClickCreate() => LnkCreate.Click();

    public void CreateProduct(Product product)
    {
        InputName.ClearAndEnterText(product.Name);
        InputDescription.ClearAndEnterText(product.Description);
        InputPrice.ClearAndEnterText(product.Price.ToString());
        SelectProductType.SelectDropDownByText(product.ProductType.ToString());
        BtnCreate.Click();
    }

    public void GoToProductDetails(string name)
    {
        PerformActionForProduct(name, TableActions.Details);
        //_driverFixture.Driver.performActionOnTable(name, TableActions.Details);
    }
    
    public void GoToEditDetails(string name)
    {
        PerformActionForProduct(name, TableActions.Edit);
        //_driverFixture.Driver.performActionOnTable(name, TableActions.Edit);
    }
    
    public void GoDeleteProduct(string name)
    {
        PerformActionForProduct(name, TableActions.Delete);
        //_driverFixture.Driver.performActionOnTable(name, TableActions.Delete);
    }
}