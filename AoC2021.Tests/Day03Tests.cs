namespace AoC2021.Tests;

[TestClass]
public class Day03Tests : DayTestsBase<Day03.Day03>
{
    public Day03Tests() : base("2583164", "2784375") { }

    [TestMethod]
    public async Task Part1WithTestData()
    {
        var day = new Day03.Day03("TestData\\Day03-testinput.txt");
        var result = await day.GetAnswerPart1();
        Assert.AreEqual("198", result);
    }

    [TestMethod]
    public async Task Part2WithTestData()
    {
        var day = new Day03.Day03("TestData\\Day03-testinput.txt");
        var result = await day.GetAnswerPart2();
        Assert.AreEqual("230", result);
    }
}