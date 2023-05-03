using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EAFramework.Config;

public static class ConfigReader
{
    public static TestSettings ReadConfig()
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
        return JsonSerializer.Deserialize<TestSettings>(configFile, jsonSerializerOptions)!;
    }
}