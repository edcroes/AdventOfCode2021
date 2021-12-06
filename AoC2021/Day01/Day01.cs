namespace AoC2021.Day01;

public class Day01 : IMDay
{
    public async Task<string> GetAnswerPart1()
    {
        var depths = await GetDepths();

        return Enumerable.Range(1, depths.Length - 1)
            .Select(i => depths[i] - depths[i - 1])
            .Count(d => d > 0)
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var depths = await GetDepths();
        var windows = Enumerable.Range(2, depths.Length - 2)
            .Select(i => depths[i] + depths[i - 1] + depths[i - 2])
            .ToArray();

        return Enumerable.Range(1, windows.Length - 1)
            .Select(i => windows[i] - windows[i - 1])
            .Count(d => d > 0)
            .ToString();
    }

    private static async Task<int[]> GetDepths() =>
        (await File.ReadAllLinesAsync("Day01\\input.txt"))
            .Where(l => !string.IsNullOrEmpty(l))
            .Select(l => int.Parse(l))
            .ToArray();
}