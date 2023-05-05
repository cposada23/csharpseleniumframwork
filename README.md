# C# Selenium framework with SpecFlow

## Framework Structure:



## Libraries

- Selenium.Webdriver
- Selenium.Support


### Basic Instructions

- Create a new solution ( Class Library )
- Install the libraries
- Right click the solution and add a new project ( this will be the test project using xunit )
- Right click the test project, select add reference and reference the framework project


### Test Settings


### Driver factory
This will help to initialize the right driver

`EAFramework/Diver/DriverFixture.cs`
```c#
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

```



### XUnit stuff

#### [Theory]
you can use this annotation along with `[InlineData]` to pass data to the test like this

```c#
    [Theory]
    [InlineData("Keyboard")]
    [InlineData("Mouse")]
    public void GoToProductDetails(string productName)
    {
        var homePage = new HomePage(_driverFixture);
        var productPage = new ProductPage(_driverFixture);
        homePage.ClickProduct();
        
        productPage.GoToProductDetails(productName);
        Thread.Sleep(6000);

    }
```

#### Passing Data Via concrete types

#### [AutoData]
Automatically generate data for the test (Use with caution)

> Install the dependency `AutoFixture.Xunit2`


This will Generate a new product automatically and pass it to the test
```c#
[Theory]
[AutoData]
public void UsingAutoFixture(Product product)
{
    var homePage = new HomePage(_driverFixture);
    var productPage = new ProductPage(_driverFixture);

    homePage.ClickProduct();
    productPage.ClickCreate();
    productPage.CreateProduct(product);
    Thread.Sleep(6000);
    productPage.GoToProductDetails(product.Name!);
}
```



### Configuration Reader


```c#
public static class ConfigReader
{
    public static TestSettings? ReadConfig()
    {
        // Getting the file from where the bin folder exists
        var configFile = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/appSettings.json");


        var jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        
        // In our case we need to convert the string for the browser that comes from the appSettings.json to an enum... so this will help
        jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // Deserialize: convert the file into a TestSettings object
        return JsonSerializer.Deserialize<TestSettings>(configFile, jsonSerializerOptions);
    }
}
```

In my case the testSettings Class looks like this:

```c#
public class TestSettings
{
    public BrowserType BrowserType { get; set; }
    public Uri? ApplicationUrl { get; set; }
    public float? TimeoutInterval { get; set; }
}
```

> Note: for the appSettings.json file to be copied into the bin folder, you need to tell the compiler to add it there. To do this you right Click the appSettings.json file -> properties and in the Copy to output directory, select Copy always

![Screenshot 2023-05-02 at 9 40 12 PM](https://user-images.githubusercontent.com/7946622/235823424-02972843-9071-4a90-8f3f-e19c7a343b3a.png)

```json
{  
	"BrowserType": "Chrome",  
	"ApplicationUrl": "http://localhost:8000",  
	"TimeoutInterval": 30  
}
```

### Building automatic waiting

> Lazy Types in C# https://learn.microsoft.com/en-us/dotnet/api/system.lazy-1?view=net-8.0


```c#
public class DriverWait : IDriverWait
{
    private readonly IDriverFixture _driverFixture;
    private readonly TestSettings _testSettings;
    private readonly Lazy<WebDriverWait> _webDriverWait;

    public DriverWait(IDriverFixture driverFixture, TestSettings testSettings)
    {
        _driverFixture = driverFixture;
        _testSettings = testSettings;
        _webDriverWait = new Lazy<WebDriverWait>(GetWebDriverWait);
    }

    public IWebElement FindElement(By elementLocator)
    {
        return _webDriverWait.Value.Until(_ => _driverFixture.Driver.FindElement(elementLocator));
    }
    
    public IEnumerable<IWebElement> FindElements(By elementLocator)
    {
        return _webDriverWait.Value.Until(_ => _driverFixture.Driver.FindElements(elementLocator));
    }

    private WebDriverWait GetWebDriverWait()
    {
        return new WebDriverWait(_driverFixture.Driver,
            timeout: TimeSpan.FromSeconds(_testSettings.TimeoutInterval ?? 30))
        {
            PollingInterval = TimeSpan.FromSeconds(_testSettings.PollingInterval ?? 1)
        };
    }
}

```


### Dependency Injection

> Package: `Xunit.DependencyInjection`

Startup.cs file, Here you tell the services what to initialized so it can be injected in the tests

```c#
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(ConfigReader.ReadConfig())
            .AddScoped<IDriverFixture, DriverFixture>()
            .AddScoped<IDriverWait, DriverWait>()
            .AddScoped<IHomePage, HomePage>()
            .AddScoped<IProductPage, ProductPage>();
    }

}
```

#### Separation of concerns
> https://deviq.com/principles/separation-of-concerns

Now that we are using dependency injection, we separated the concerns from the test class ( driver creation, pages initialization, fixtures.... etc) and now the test class only concern is the test it self ( It does not and should not care on how the diver is being created, how to instantiate a page... etc)
Test class:
```c#
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
```

## Integrating specflow

add a new project -> SpecFlow

![Screenshot 2023-05-03 at 3 19 00 PM](https://user-images.githubusercontent.com/7946622/236040626-cf24002f-ef33-43c8-b547-9dcece873adf.png)

#### Working with tables

> To use dynamic data use the package specflow.assist.dynamic this will help you to create dynamic types from the table

Example of tables

`.feature`

```
Feature: Specflow examples
    Scenario: Working with tables
        Given I input following numbers to the calculator
            | Number |
            | 123     |
            | 423     |
        When I add them the results should be
            | Result | Symbol |
            | 576    | +      |
```

specDefinition:

```c#
[Binding]
public class SpecFlowExamplesStepDefinitions
{
    [Given(@"I input following numbers to the calculator")]
    public void GivenIInputFollowingNumbersToTheCalculator(Table table)
    {
        // Using my own type
        var data = table.CreateSet<Calculation>();

        foreach (var item in data)
        {
            Console.WriteLine($"The number is {item.Number}");
            item.Number.Should().BePositive().And.BeGreaterThan(0);
        }
        
        // Using dynamic type
        dynamic dynamicData = table.CreateDynamicSet();
        
        foreach (var item in dynamicData)
        {
            Console.WriteLine($"The number is {item.Number}");
            int number = int.Parse(item.Number.ToString());
            number.Should().BePositive().And.BeGreaterThan(0);
        }
    }
    
    [When(@"I add them the results should be")]
    public void WhenIAddThemTheResultsShouldBe(Table table)
    {
        // Using my own type
        var result = table.CreateInstance<Res>();
        result.Result.Should().Be(576);
        result.Symbol.Should().Be("+");
        
        // Using dynamic type
        dynamic dynamicResult = table.CreateDynamicInstance();

        int nResult = (int.Parse(dynamicResult.Result.ToString()));
        nResult.Should().Be(576);
        string symbol = dynamicResult.Symbol.ToString();
        symbol.Should().Be("+");
    }
    
    private record Calculation
    {
        public int Number { get; set; }
    }

    private record Res
    {
        public int Result { get; set; }
        public string Symbol { get; set; }
    }
}
```

#### Hooks

-  [BeforeTestRun] / [AfterTestRun]: The method you applied these ones should be `static`. Runs before/after the entire test run
-  [BeforeFeature ] / [AfterFeature]: Runs before/after executing each individual feature
-  [BeforeScenario] or [Before] / [AfterScenario] or [After] Runs before/after each individual Scenario
-  [BeforeScenarioBlock] / [AfterScenarioBlock] Runs before/after each scenario block (Given, When, Then ... )
-  [BeforeStep] / [AfterStep] runs before/after executing each scenario step




#### Selenium Grid
> https://www.selenium.dev/documentation/grid/

```bash
java - jar selenium-server-x.x.x.jar standalone
```

Open: http://localhost:4444/ui


> Remember to install Chrome driver in you machine: https://www.swtestacademy.com/install-chrome-driver-on-mac/

Use RemoteWebDriver

