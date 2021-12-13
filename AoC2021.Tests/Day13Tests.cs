namespace AoC2021.Tests;

[TestClass]
public class Day13Tests : DayTestsWithTestDataBase<Day13.Day13>
{
    private const string AnswerPart2 = "\r\n" +
        "#.....##..#..#.####..##..#..#.####...##.\r\n" +
        "#....#..#.#..#.#....#..#.#..#.#.......#.\r\n" +
        "#....#....####.###..#....#..#.###.....#.\r\n" +
        "#....#.##.#..#.#....#.##.#..#.#.......#.\r\n" +
        "#....#..#.#..#.#....#..#.#..#.#....#..#.\r\n" +
        "####..###.#..#.####..###..##..####..##..\r\n";

    private const string AnswerTestDataPart2 = "\r\n" +
        "#####\r\n" +
        "#...#\r\n" +
        "#...#\r\n" +
        "#...#\r\n" +
        "#####\r\n" +
        ".....\r\n" +
        ".....\r\n";

    public Day13Tests() : base("807", AnswerPart2, "17", AnswerTestDataPart2) { }
}