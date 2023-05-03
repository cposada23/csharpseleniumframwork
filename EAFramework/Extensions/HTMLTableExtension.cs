using System.ComponentModel.DataAnnotations;
using OpenQA.Selenium;

namespace EAFramework.Extensions;

public static class HtmlTableExtension
{
    public static void performActionOnTable(this IWebDriver driver, string productName, TableActions action, string? xpath = null)
    {
        xpath ??= $"//table//tbody//td[contains(text(), '{productName}')]/..//td/a[contains(text(), '{action.ToString()}')]";
        Console.WriteLine(xpath);
        driver.FindElement(By.XPath(xpath)).Click();
    }
}

public enum TableActions
{
    [Display(Name = "Edit")]
    Edit,
    
    [Display(Name = "Details")]
    Details,
    
    [Display(Name = "Delete")]
    Delete,
}