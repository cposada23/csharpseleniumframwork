using EAEspecflowTest.Models;
using EAEspecflowTest.Pages;
using FluentAssertions;
using TechTalk.SpecFlow.Assist;

namespace EAEspecflowTest.Steps;

[Binding]
public sealed class ProductStepDefinitions
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private readonly ScenarioContext _scenarioContext;
    private readonly IHomePage _homePage;
    private readonly IProductPage _productPage;

    public ProductStepDefinitions(ScenarioContext scenarioContext, IHomePage homePage, IProductPage productPage)
    {
        _scenarioContext = scenarioContext;
        _homePage = homePage;
        _productPage = productPage;
    }
    
    [Given(@"I click the Product menu")]
    public void GivenIClickTheProductMenu()
    {
        Console.WriteLine($"Executing step: {nameof(GivenIClickTheProductMenu)}");
        _homePage.ClickProduct();
    }


    [Given(@"I click the ""(.*)"" link")]
    public void GivenIClickTheLink(string link)
    {
        _productPage.ClickCreate();
    }

    [Given(@"i create a Product with the following details")]
    public void GivenICreateAProductWithTheFollowingDetails(Table table)
    {
        var product = table.CreateInstance<Product>();
        _productPage.CreateProduct(product);
        _scenarioContext.Set<Product>(product);
    }

    [When(@"I click the Details link of the newly created product")]
    public void WhenIClickTheDetailsLinkOfTheNewlyCreatedProduct()
    {
        var product = _scenarioContext.Get<Product>();
        _productPage.GoToProductDetails(product.Name!);
    }

    [Then(@"I see all the product details are created as expected")]
    public void ThenISeeAllTheProductDetailsAreCreatedAsExpected()
    {
        var name = _productPage.GetProductName();
        var product = _scenarioContext.Get<Product>();

        name.Should().BeEquivalentTo(product.Name!.Trim());

    }
}