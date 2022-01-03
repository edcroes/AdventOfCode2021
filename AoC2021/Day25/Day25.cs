namespace AoC2021.Day25;

public class Day25 : IMDay
{
    public string FilePath { private get; init; } = "Day25\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var map = await GetMap();
        var moves = 0;
        while (TryMove(map))
        {
            moves++;
        }

        return (++moves).ToString();
    }

    public Task<string> GetAnswerPart2()
    {
        return Task.FromResult("The end!");
    }

    private static bool TryMove(Map<char> map)
    {
        var movingEast = map
            .Where((p, v) => v == '>')
            .Where(p => CanMoveEast(map, p))
            .ToArray();

        foreach (var moving in movingEast)
        {
            MoveEast(map, moving);
        }

        var movingSouth = map
            .Where((p, v) => v == 'v')
            .Where(p => CanMoveSouth(map, p))
            .ToArray();

        foreach (var moving in movingSouth)
        {
            MoveSouth(map, moving);
        }

        return movingEast.Length > 0 || movingSouth.Length > 0;
    }

    private static bool CanMoveEast(Map<char> map, Point location)
    {
        var next = new Point(location.X + 1 < map.SizeX ? location.X + 1 : 0, location.Y);
        return map.GetValue(next) == '.';
    }

    private static bool CanMoveSouth(Map<char> map, Point location)
    {
        var next = new Point(location.X, location.Y + 1 < map.SizeY ? location.Y + 1 : 0);
        return map.GetValue(next) == '.';
    }

    private static void MoveEast(Map<char> map, Point location)
    {
        var next = new Point(location.X + 1 < map.SizeX ? location.X + 1 : 0, location.Y);
        map.SetValue(next, '>');
        map.SetValue(location, '.');
    }

    private static void MoveSouth(Map<char> map, Point location)
    {
        var next = new Point(location.X, location.Y + 1 < map.SizeY ? location.Y + 1 : 0);
        map.SetValue(next, 'v');
        map.SetValue(location, '.');
    }

    private async Task<Map<char>> GetMap() =>
        new((await File.ReadAllLinesAsync(FilePath))
            .Where(l => l.IsNotNullOrEmpty())
            .Select(l => l.ToCharArray())
            .ToArray());
}