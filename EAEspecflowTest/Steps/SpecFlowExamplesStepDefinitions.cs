using FluentAssertions;
using TechTalk.SpecFlow.Assist;

namespace EAEspecflowTest;

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