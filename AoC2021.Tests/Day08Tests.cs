namespace AoC2021.Tests;

[TestClass]
public class Day08Tests : DayTestsBase<Day08.Day08>
{
    public Day08Tests() : base("310", "915941") { }

    [TestMethod]
    public async Task Part1WithTestData()
    {
        var day = new Day08.Day08("TestData\\Day08-testinput.txt");
        var result = await day.GetAnswerPart1();
        Assert.AreEqual("26", result);
    }

    [TestMethod]
    public async Task Part2WithTestData()
    {
        var day = new Day08.Day08("TestData\\Day08-testinput.txt");
        var result = await day.GetAnswerPart2();
        Assert.AreEqual("61229", result);
    }
}