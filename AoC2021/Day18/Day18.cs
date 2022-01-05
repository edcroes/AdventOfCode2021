namespace AoC2021.Day18;

public class Day18 : IMDay
{
    public string FilePath { private get; init; } = "Day18\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var numbers = await GetNumbers();
        var first = numbers.First();

        foreach (var number in numbers.Skip(1))
        {
            first.Add(number);
        }

        return first.Magnitude.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var numbers = await GetNumbers();
        var maxMagnitude = 0;

        foreach (var number in numbers)
        {
            foreach (var other in numbers.Where(n => n != number))
            {
                maxMagnitude = Math.Max(maxMagnitude, (number + other).Magnitude);
            }
        }

        return maxMagnitude.ToString();
    }

    private async Task<IEnumerable<SnailfishNumber>> GetNumbers() =>
        (await File.ReadAllLinesAsync(FilePath))
            .Where(l => l.IsNotNullOrEmpty())
            .Select(l => new SnailfishNumber(l));
}
