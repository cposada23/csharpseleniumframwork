using AutoFixture.Xunit2;
using EAFramework.Diver;
using TestProject1.Models;
using TestProject1.Pages;

namespace TestProject1;

public class UnitTest3
{

    private readonly IHomePage _homePage;
    private readonly IProductPage _productPage;
    
    public UnitTest3(
        IHomePage homePage,
        IProductPage productPage
    )
    {
        _homePage = homePage;
        _productPage = productPage;
    }
    
    [Fact]
    public void Test1()
    {
        
        var product = new Product()
        {
            Name = "testNewProduct",
            Description = "New Description",
            Price = 200,
            ProductType = ProductType.MONITOR
        };
        
        _homePage.ClickProduct();
        _productPage.ClickCreate();
        _productPage.CreateProduct(product);
        Thread.Sleep(6000);
    }
    
}