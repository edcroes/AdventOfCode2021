namespace AoC2021.Day09;

public class Day09 : IMDay
{
    private const int Top = 9;
    private Map<int> _map;
    public string FilePath { private get; init; } = "Day09\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        await Init();
        var lowestPoints = GetLowestPoints();
        var result = lowestPoints.Select(p => _map.GetValue(p) + 1).Sum();

        return result.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        await Init();
        var lowestPoints = GetLowestPoints();
        var result = lowestPoints
            .Select(GetBasin)        
            .Select(b => b.Count)
            .OrderByDescending(c => c)
            .Take(3)
            .Aggregate((first, second) => first * second);
        
        return result.ToString();
    }

    private List<Point> GetLowestPoints() =>
        _map
            .Where((p, v) => _map.NumberOfStraightNeighborsThatMatch(p, (current, neighbor) => current < neighbor, 10) == 4)
            .ToList();

    private List<Point> GetBasin(Point point)
    {
        List<Point> basinPoints = new() { point };
        List<Point> nextPoints = new() { point };

        while (nextPoints.Any())
        {
            nextPoints = nextPoints
                .SelectMany(p => _map.GetStraightNeighbors(p).Where(n => IsHigherButNotTop(p, n)))
                .Distinct()
                .Where(p => !basinPoints.Contains(p))
                .ToList();
            basinPoints.AddRange(nextPoints);
        }

        return basinPoints;
    }

    private bool IsHigherButNotTop(Point first, Point second) =>
        _map.GetValue(second) > _map.GetValue(first) && _map.GetValue(second) < Top;

    private async Task Init() =>
        _map = new((await File.ReadAllLinesAsync(FilePath))
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => l.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray())
            .ToArray());
}
