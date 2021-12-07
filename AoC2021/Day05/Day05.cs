namespace AoC2021.Day05;

public class Day05 : IMDay
{
    private readonly string _linesFile;

    public Day05(string linesFile)
    {
        _linesFile = linesFile;
    }

    public Day05() : this("Day05\\input.txt") { }

    public async Task<string> GetAnswerPart1()
    {
        var lines = await GetLines();
        lines = lines.Where(l => l.From.X == l.To.X || l.From.Y == l.To.Y);

        var result = CountPointsTouchedMultipleTimes(lines);
        return result.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var lines = await GetLines();

        var result = CountPointsTouchedMultipleTimes(lines);
        return result.ToString();
    }

    private static int CountPointsTouchedMultipleTimes(IEnumerable<Line> lines)
    {
        Dictionary<Point, int> pointsTouched = new();
        foreach (var line in lines)
        {
            foreach (var point in line.GetLinePoints())
            {
                AddOrRaisePoint(pointsTouched, point);
            }
        }

        return pointsTouched.Values.Count(v => v > 1);
    }

    private static void AddOrRaisePoint(Dictionary<Point, int> dictionary, Point point)
    {
        if (dictionary.ContainsKey(point))
        {
            dictionary[point]++;
        }
        else
        {
            dictionary.Add(point, 1);
        }
    }

    private async Task<IEnumerable<Line>> GetLines() =>
        (await File.ReadAllLinesAsync(_linesFile))
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => l.Split(" -> "))
            .Select(a => new Line(ParsePoint(a[0]), ParsePoint(a[1])));

    private static Point ParsePoint(string point) =>
        new (int.Parse(point.Split(',')[0]), int.Parse(point.Split(',')[1]));
}
