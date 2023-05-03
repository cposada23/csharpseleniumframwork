using EAFramework.Diver;

namespace EAFramework.Config;

public class TestSettings
{
    public BrowserType BrowserType { get; set; }
    public Uri? ApplicationUrl { get; set; }
    public float? TimeoutInterval { get; set; }
    
    public int? PollingInterval { get; set; }


}