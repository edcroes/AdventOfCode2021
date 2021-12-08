namespace AoC2021.Day08;

public class Day08 : IMDay
{
    private readonly string _file;

    public Day08(string file)
    {
        _file = file;
    }

    public Day08() : this("Day08\\input.txt") { }

    public async Task<string> GetAnswerPart1()
    {
        var displays = await GetDisplays();
        int[] simpleNumbers = new[] { 2, 3, 4, 7 };

        return displays
                .Select(d => d.Digits.Count(d => simpleNumbers.Contains(d.Length)))
                .Sum()
                .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var displays = await GetDisplays();
        return displays
                .Select(d => d.DisplayedNumber)
                .Sum()
                .ToString();
    }

    private async Task<IEnumerable<Display>> GetDisplays() =>
        (await File.ReadAllLinesAsync(_file))
            .Select(l => l.Split("|"))
            .Select(a => new Display(a[0].Split(' ', StringSplitOptions.RemoveEmptyEntries), a[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)));
}
