using EAFramework.Config;
using EAFramework.Diver;
using Microsoft.Extensions.DependencyInjection;
using TestProject1.Pages;

namespace TestProject1;

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