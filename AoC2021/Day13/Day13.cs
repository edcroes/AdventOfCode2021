namespace AoC2021.Day13;

public class Day13 : IMDay
{
    public string FilePath { private get; init; } = "Day13\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var (map, foldingPoints) = await GetPaper();
        var firstFoldingPoint = foldingPoints.First();
        map = FoldMap(map, firstFoldingPoint);

        return map.Count(true).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var (map, foldingPoints) = await GetPaper();
        foreach (var foldingPoint in foldingPoints)
        {
            map = FoldMap(map, foldingPoint);
        }

        return GetMapDump(map);
    }

    private static Map<bool> FoldMap(Map<bool> map, Point foldingPoint)
    {
        var firstMapTo = foldingPoint.X > 0
            ? new Point(foldingPoint.X - 1, map.SizeY - 1)
            : new Point(map.SizeX - 1, foldingPoint.Y - 1);

        var secondMapFrom = foldingPoint.X > 0
            ? new Point(foldingPoint.X + 1, 0)
            : new Point(0, foldingPoint.Y + 1);

        var newMap = map.GetSubMap(new Point(0, 0), firstMapTo);
        var foldedPart = map.GetSubMap(secondMapFrom, new Point(map.SizeX - 1, map.SizeY - 1));
        if (foldingPoint.X > 0)
        {
            _ = foldedPart.MirrorHorizontal();
        }
        else
        {
            _ = foldedPart.MirrorVertical();
        }

        newMap.ForEach((p, v) => newMap.SetValue(p, v | foldedPart.GetValue(p)));

        return newMap;
    }

    private static string GetMapDump(Map<bool> map)
    {
        StringBuilder mapDump = new();
        mapDump.AppendLine();

        for (var y = 0; y < map.SizeY; y++)
        {
            for (var x = 0; x < map.SizeX; x++)
            {
                mapDump.Append(map.GetValue(x, y) ? "#" : ".");
            }
            mapDump.AppendLine();
        }

        return mapDump.ToString();
    }

    private async Task<(Map<bool>, List<Point>)> GetPaper()
    {
        var lines = await File.ReadAllLinesAsync(FilePath);

        var points = lines
            .Where(l => !l.StartsWith("fold"))
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => l.Split(','))
            .Select(l => new Point(int.Parse(l[0]), int.Parse(l[1])));
        var map = CreateMap(points);

        var foldingPoints = lines
            .Where(l => l.StartsWith("fold"))
            .Select(l => l.Replace("fold along ", ""))
            .Select(p => p.Split("="))
            .Select(p => p[0] switch
            {
                "y" => new Point(0, int.Parse(p[1])),
                "x" => new Point(int.Parse(p[1]), 0),
                _ => throw new InvalidOperationException($"Fold along {p[0]} is impossible")
            })
            .ToList();

        return (map, foldingPoints);
    }

    private static Map<bool> CreateMap(IEnumerable<Point> points)
    {
        Map<bool> map = new(points.Max(p => p.X + 1), points.Max(p => p.Y + 1));
        foreach (var point in points)
        {
            map.SetValue(point, true);
        }

        return map;
    }
}
