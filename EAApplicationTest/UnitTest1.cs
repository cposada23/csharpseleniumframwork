using AutoFixture.Xunit2;
using EAFramework.Diver;
using TestProject1.Models;
using TestProject1.Pages;

namespace TestProject1;

public class UnitTest1
{

    private readonly IHomePage _homePage;
    private readonly IProductPage _productPage;
    
    public UnitTest1(
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

    [Theory]
    [InlineData( "Keyboard")]
    [InlineData("Mouse")]
    public void GoToProductDetails(string productName)
    {

        _homePage.ClickProduct();
        
        _productPage.GoToProductDetails(productName);
        Thread.Sleep(6000);

    }
    
    [Theory]
    [AutoData]
    public void UsingAutoFixture(Product product)
    {

        _homePage.ClickProduct();
        _productPage.ClickCreate();
        _productPage.CreateProduct(product);
        Thread.Sleep(6000);
        _productPage.GoToProductDetails(product.Name!);
    }
}