using EAEspecflowTest.Pages;
using EAFramework.Config;
using EAFramework.Diver;
using Microsoft.Extensions.DependencyInjection;
using SolidToken.SpecFlow.DependencyInjection;

namespace EAEspecflowTest;

public class Startup
{
    [ScenarioDependencies]
    public static IServiceCollection CreateServices()
    {
        var services = new ServiceCollection();

        services
            .AddSingleton(ConfigReader.ReadConfig())
            .AddScoped<IDriverFixture, DriverFixture>()
            .AddScoped<IDriverWait, DriverWait>()
            .AddScoped<IHomePage, HomePage>()
            .AddScoped<IProductPage, ProductPage>();

        return services;
    }
    
} 