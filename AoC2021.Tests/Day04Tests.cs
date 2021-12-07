namespace AoC2021.Tests;

[TestClass]
public class Day04Tests : DayTestsBase<Day04.Day04>
{
    public Day04Tests() : base("38913", "16836") { }

    [TestMethod]
    public async Task Part1WithTestData()
    {
        var day = new Day04.Day04("TestData\\Day04-testinput.txt");
        var result = await day.GetAnswerPart1();
        Assert.AreEqual("4512", result);
    }

    [TestMethod]
    public async Task Part2WithTestData()
    {
        var day = new Day04.Day04("TestData\\Day04-testinput.txt");
        var result = await day.GetAnswerPart2();
        Assert.AreEqual("1924", result);
    }
}