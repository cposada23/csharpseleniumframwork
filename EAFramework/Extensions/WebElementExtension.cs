using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace EAFramework.Extensions;

public static class WebElementExtension
{
    public static void SelectDropDownByText(this IWebElement element, string text)
    {
        var select = new SelectElement(element);
        select.SelectByText(text);
    }

    public static void ClearAndEnterText(this IWebElement element, string? text = "")
    {
        element.Clear();
        element.SendKeys(text);
    }
}