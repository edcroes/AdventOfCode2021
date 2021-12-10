namespace AoC2021.Tests;

[TestClass]
public class Day09Tests : DayTestsWithTestDataBase<Day09.Day09>
{
    public Day09Tests() : base("554", "1017792", "15", "1134") { }

    [TestMethod]
    public async Task Part2WithDataFromMarcelTest()
    {
        var day = new Day09.Day09 { FilePath = "TestData\\Day09-Marcel.txt" };
        var answer = await day.GetAnswerPart2();
        Assert.AreEqual("949905", answer);
    }
}