namespace AoC2021.Common.Maps;

public static class MapExtensions
{
    public static Point First<T>(this Map<T> map, Func<Point, T, bool> predicate)
    {
        for (var y = 0; y < map.SizeY; y++)
        {
            for (var x = 0; x < map.SizeX; x++)
            {
                var point = new Point(x, y);
                if (predicate(point, map.GetValue(point)))
                {
                    return point;
                }
            }
        }

        throw new Exception();
    }

    public static Point Last<T>(this Map<T> map, Func<Point, T, bool> predicate)
    {
        for (var y = map.SizeY - 1; y >= 0; y--)
        {
            for (var x = map.SizeX - 1; x >= 0; x--)
            {
                var point = new Point(x, y);
                if (predicate(point, map.GetValue(point)))
                {
                    return point;
                }
            }
        }

        throw new Exception();
    }

    public static IEnumerable<Point> Where<T>(this Map<T> map, Func<Point, T, bool> predicate)
    {
        var points = new List<Point>();
        for (var y = 0; y < map.SizeY; y++)
        {
            for (var x = 0; x < map.SizeX; x++)
            {
                var point = new Point(x, y);
                if (predicate(point, map.GetValue(point)))
                {
                    points.Add(point);
                }
            }
        }

        return points;
    }

    public static int Count<T>(this Map<T> map, T valueToMatch)
    {
        int count = 0;
        for (int y = 0; y < map.SizeY; y++)
        {
            for (int x = 0; x < map.SizeX; x++)
            {
                if (map.GetValue(x, y).Equals(valueToMatch))
                {
                    count++;
                }
            }
        }

        return count;
    }
}