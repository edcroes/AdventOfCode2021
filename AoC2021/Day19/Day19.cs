namespace AoC2021.Day19;

public class Day19 : IMDay
{
    private const int MinimalOverlapCount = 12;
    private readonly static Action<Space3D>[] NextOrientation = new Action<Space3D>[]
    {
        s => { },
        s => s.RotateX(),
        s => s.RotateX(),
        s => s.RotateX(),
        s => s.RotateX(),
        s => s.RotateY(),
        s => s.RotateZ(),
        s => s.RotateZ(),
        s => s.RotateZ(),
        s => s.RotateY(),
        s => s.RotateX(),
        s => s.RotateX(),
        s => s.RotateX(),
        s => s.RotateY(),
        s => s.RotateZ(),
        s => s.RotateZ(),
        s => s.RotateZ(),
        s => s.RotateX(),
        s => s.RotateY(),
        s => s.RotateY(),
        s => s.RotateY(),
        s => s.RotateX().RotateX(),
        s => s.RotateY(),
        s => s.RotateY(),
        s => s.RotateY()
    };

    private readonly Dictionary<Space3D, List<Point3D>> _nonMatchingPoints = new();

    public string FilePath { private get; init; } = "Day19\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        _nonMatchingPoints.Clear();
        var allScanners = await GetScanners();
        MapAllScanners(allScanners);

        var result = allScanners.SelectMany(p => p.Points).Distinct().Count();
        return result.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        _nonMatchingPoints.Clear();
        var allScanners = await GetScanners();
        MapAllScanners(allScanners);

        var maxDistance = 0;
        foreach (var scanner in allScanners)
        {
            foreach (var other in allScanners.Where(s => s != scanner))
            {
                maxDistance = Math.Max(maxDistance, scanner.Center.GetManhattenDistance(other.Center));
            }
        }

        return maxDistance.ToString();
    }

    private void MapAllScanners(List<Space3D> allScanners)
    {
        var mainScanner = allScanners.First();
        var otherScanners = allScanners.Skip(1).ToList();

        while (otherScanners.Any())
        {
            foreach (var other in otherScanners.ToArray())
            {
                if (Map(mainScanner, other))
                {
                    mainScanner.AddRange(other.Points);
                    otherScanners.Remove(other);
                }
                else
                {
                    _nonMatchingPoints.AddOrUpdate(other, mainScanner.Points);
                }
            }
        }
    }

    private bool Map(Space3D main, Space3D other)
    {
        Console.Write($"Trying to map {other.Name} to {main.Name}");

        var orientation = 0;
        var mapped = false;
        while (orientation < NextOrientation.Length && !mapped)
        {
            NextOrientation[orientation++](other);
            mapped = MapOrientation(main, other);
        }

        Console.WriteLine(mapped ? $"  MAPPED" : "  -" );
        
        return mapped;
    }

    private bool MapOrientation(Space3D main, Space3D other)
    {
        var pointsToCheck = main.Points.Where(p => !_nonMatchingPoints.ContainsKey(other) || !_nonMatchingPoints[other].Contains(p)).ToArray();

        for (var otherIndex = 0; otherIndex < other.CornerPoints.Count; otherIndex++)
        {
            for (var mainIndex = 0; mainIndex < pointsToCheck.Length; mainIndex++)
            {
                other.MoveBy(pointsToCheck[mainIndex] - other.CornerPoints[otherIndex]);
                var overlapCount = main.Points.Count(p => other.Points.Contains(p));

                if (overlapCount >= MinimalOverlapCount)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private async Task<List<Space3D>> GetScanners() =>
        (await File.ReadAllTextAsync(FilePath))
            .Trim()
            .Replace("\r\n", "\n")
            .Split("\n\n")
            .Select(s => ParseScanner(s.Trim()))
            .ToList();

    private static Space3D ParseScanner(string scanner)
    {
        var name = scanner.Split('\n')[0].Trim('-').Trim();
        return new(scanner
            .Split('\n')
            .Skip(1)
            .Select(p => p
                .Split(',')
                .Select(i => int.Parse(i))
                .ToArray())
            .Select(p => new Point3D(p[0], p[1], p[2])))
        {
            Name = name
        };
    }
 }
