namespace AoC2021.Tests;

[TestClass]
public class Day06Tests : DayTestsBase<Day06.Day06>
{
    public Day06Tests() : base("362639", "1639854996917") { }

    [TestMethod]
    public async Task Part1WithTestData()
    {
        var day06 = new Day06.Day06("TestData\\Day06-testinput.txt");
        var result = await day06.GetAnswerPart1();
        Assert.AreEqual("5934", result);
    }

    [TestMethod]
    public async Task Part2WithTestData()
    {
        var day06 = new Day06.Day06("TestData\\Day06-testinput.txt");
        var result = await day06.GetAnswerPart2();
        Assert.AreEqual("26984457539", result);
    }
}