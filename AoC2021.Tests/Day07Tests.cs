namespace AoC2021.Tests;

[TestClass]
public class Day07Tests : DayTestsBase<Day07.Day07>
{
    public Day07Tests() : base("348996", "98231647") { }

    [TestMethod]
    public async Task Part1WithTestData()
    {
        var day = new Day07.Day07("TestData\\Day07-testinput.txt");
        var result = await day.GetAnswerPart1();
        Assert.AreEqual("37", result);
    }

    [TestMethod]
    public async Task Part2WithTestData()
    {
        var day = new Day07.Day07("TestData\\Day07-testinput.txt");
        var result = await day.GetAnswerPart2();
        Assert.AreEqual("168", result);
    }
}