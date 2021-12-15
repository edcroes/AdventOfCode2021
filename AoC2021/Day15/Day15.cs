namespace AoC2021.Day15;

public class Day15 : IMDay
{
    private Map<int> _map;

    public string FilePath { private get; init; } = "Day15\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        await Init();
        var end = new Point(_map.SizeX - 1, _map.SizeY - 1);
        var result = _map.GetShortestPath(end);

        return result.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        await Init();
        var biggerMap = CreateBiggerMap(5);
        var end = new Point(biggerMap.SizeX - 1, biggerMap.SizeY - 1);
        var result = biggerMap.GetShortestPath(end);

        return result.ToString();
    }

    private Map<int> CreateBiggerMap(int mutliplySizeBy)
    {
        Map<int> totalMap = new(_map.SizeX * mutliplySizeBy, _map.SizeY * mutliplySizeBy);
        var newMapPieces = new Map<int>[mutliplySizeBy * 2 - 1];

        for (var i = 0; i < mutliplySizeBy * 2 - 1; i++)
        {
            newMapPieces[i] = GetRaisedMap(i);
        }

        for (var y = 0; y < mutliplySizeBy; y++)
        {
            for (var x = 0; x < mutliplySizeBy; x++)
            {
                var point = new Point(x * _map.SizeX, y * _map.SizeY);
                newMapPieces[x + y].CopyTo(totalMap, point);
            }
        }

        return totalMap;
    }

    private Map<int> GetRaisedMap(int raiseBy)
    {
        Map<int> raisedMap = new(_map.SizeX, _map.SizeY);
        raisedMap.ForEach((p, v) => raisedMap.SetValue(p, (_map.GetValue(p) + raiseBy + 8) % 9 + 1));

        return raisedMap;
    }

    private async Task Init()
    {
        if (_map is not null)
        {
            return;
        }

        _map = new((await File.ReadAllLinesAsync(FilePath))
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => l.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray())
            .ToArray());
    }
}
