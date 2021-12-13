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

    public static Map<T> ForEach<T>(this Map<T> map, Action<Point, T> action)
    {
        for (int y = 0; y < map.SizeY; y++)
        {
            for (int x = 0; x < map.SizeX; x++)
            {
                action(new Point(x, y), map.GetValue(x, y));
            }
        }

        return map;
    }

    public static bool All<T>(this Map<T> map, Func<Point, T, bool> predicate)
    {
        for (var y = 0; y < map.SizeY; y++)
        {
            for (var x = 0; x < map.SizeX; x++)
            {
                var point = new Point(x, y);
                if (!predicate(point, map.GetValue(point)))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static Map<T> GetSubMap<T>(this Map<T> map, Point from, Point to)
    {
        if (from.X < 0 || from.X >= map.SizeX || from.Y < 0 || from.Y >= map.SizeY)
        {
            throw new ArgumentException("Range is outside the map");
        }

        if (from.X > to.X || from.Y > to.Y)
        {
            throw new ArgumentException("'from' should come before 'to'");
        }

        var newSizeX = to.X - from.X + 1;
        var newSizeY = to.Y - from.Y + 1;
        Map<T> newMap = new(newSizeX, newSizeY);

        for (var y = from.Y; y <= to.Y; y++)
        {
            for (var x = from.X; x <= to.X; x++)
            {
                newMap.SetValue(x - from.X, y - from.Y, map.GetValue(x, y));
            }
        }

        return newMap;
    }
}