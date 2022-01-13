namespace AoC2021.Day23;

public class Day23 : IMDay
{
    public string FilePath { private get; init; } = "Day23\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var map = await GetMap(FilePath);

        var burrow = GetBurrow(map);
        var desiredState = GetDesiredState(burrow);
        var state = GetInitialState(map);

        var cost = GetCostFromState(state, desiredState, burrow);

        return cost.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var map = await GetMapPart2();

        var burrow = GetBurrow(map);
        var desiredState = GetDesiredState(burrow);
        var state = GetInitialState(map);

        var cost = GetCostFromState(state, desiredState, burrow);

        return cost.ToString();
    }

    private static int GetCostFromState(State state, State desiredState, Burrow burrow)
    {
        return GetCostFromState(state, desiredState, burrow, new(), new());
    }

    private static int GetCostFromState(State state, State desiredState, Burrow burrow, Dictionary<State, int> cache, HashSet<State> deadEnds)
    {
        if (state.Equals(desiredState))
        {
            return 0;
        }

        if (cache.ContainsKey(state))
        {
            return cache[state];
        }
        
        if (deadEnds.Contains(state))
        {
            return int.MaxValue;
        }

        List<(State, int)> nextStatesAndCost = GetNextStates(state, burrow);
        if (nextStatesAndCost.Count == 0)
        {
            return int.MaxValue;
        }

        var minCost = int.MaxValue;
        foreach (var (nextState, cost) in nextStatesAndCost)
        {
            var totalCost = GetCostFromState(nextState, desiredState, burrow, cache, deadEnds);
            if (totalCost != int.MaxValue && totalCost + cost < minCost)
            {
                minCost = totalCost + cost;
            }
        }

        if (minCost != int.MaxValue)
        {
            cache.Add(state, minCost);
        }
        else
        {
            deadEnds.Add(state);
        }

        return minCost;
    }

    private static List<(State, int)> GetNextStates(State state, Burrow burrow)
    {
        List<(State, int)> nextStatesWithCost = new();
        var unfinished = state.Amphipods.Where(a => !burrow.Rooms[a.Type].IsAmphipodFinished(state, a)).ToArray();
        var movable = unfinished
            .Where(a => burrow.Rooms[a.Type].CanMoveIntoRoom(state) && IsPathClear(state, a, burrow.Rooms[a.Type].GetNextFreePoint(state), burrow))
            .Select(a => (a, new[] { burrow.Rooms[a.Type].GetNextFreePoint(state) }))
            .ToArray();

        if (movable.Length == 0)
        {
            movable = unfinished
                .Where(a => a.Location.Y != burrow.Hallway.Y && state.Amphipods.All(o => o == a || o.Location.X != a.Location.X || o.Location.Y > a.Location.Y))
                .Select(a => (a, burrow.Hallway.Points
                    .Where(p => p.X != a.Location.X && state.Amphipods.All(o => o.Location != p))
                    .Where(p => IsPathClear(state, a, p, burrow)).ToArray()))
                .ToArray();
        }

        foreach (var (amphipod, freeSpots) in movable)
        {
            foreach (var spot in freeSpots)
            {
                var newState = new State(state.Amphipods, amphipod, amphipod with { Location = spot });
                var steps = (amphipod.Location.Y - burrow.Hallway.Y) + (spot.Y - burrow.Hallway.Y) + Math.Abs(spot.X - amphipod.Location.X);
                var cost = steps * burrow.TypeCost[amphipod.Type];

                nextStatesWithCost.Add((newState, cost));
            }
        }

        return nextStatesWithCost;
    }

    private static bool IsPathClear(State state, Amphipod amphipod, Point to, Burrow burrow) =>
        !state.Amphipods.Any(a =>
            a != amphipod &&
            (
                a.Location.Y == burrow.Hallway.Y && a.Location.X >= Math.Min(amphipod.Location.X, to.X) && a.Location.X <= Math.Max(amphipod.Location.X, to.X) ||
                (a.Location.Y < amphipod.Location.Y && a.Location.X == amphipod.Location.X) ||
                (a.Location.Y <= to.Y && a.Location.X == to.X)
            )
        );

    private static State GetInitialState(Map<char> map) =>
        new(map
            .Where((p, v) => v is not '.' and not '#' and not ' ')
            .Select(p => new Amphipod(map.GetValue(p), p)));

    private static State GetDesiredState(Burrow burrow) =>
        new(burrow.Rooms.Values.SelectMany(r => r.Points.Select(p => new Amphipod(r.Type, p))));

    private static Burrow GetBurrow(Map<char> map)
    {
        var rooms = GetRooms(map);
        var hallway = GetHallway(map);
        var typeCost = GetTypeCost(map);

        return new(rooms, hallway, typeCost);
    }

    private static Dictionary<char, int> GetTypeCost(Map<char> map)
    {
        var typePoints = map.Where((p, v) => v is not '.' and not '#' and not ' ');
        var types = typePoints
            .Select(p => map.GetValue(p))
            .Distinct()
            .OrderBy(t => t)
            .ToArray();

        return Enumerable.Range(0, types.Length)
            .Select(i => (types[i], (int)(1 * Math.Pow(10, i))))
            .ToDictionary(p => p.Item1, p => p.Item2);
    }

    private static Dictionary<char, Room> GetRooms(Map<char> map)
    {
        var roomPoints = map.Where((p, v) => v is not '.' and not '#' and not ' ');

        var types = roomPoints
            .Select(p => map.GetValue(p))
            .Distinct()
            .OrderBy(t => t)
            .ToArray();

        var roomXs = roomPoints.Select(p => p.X).Distinct().OrderBy(x => x).ToArray();
        Dictionary<char, Room> rooms = Enumerable.Range(0, roomXs.Length)
            .Select(i => (types[i], roomPoints.Where(p => p.X == roomXs[i]).OrderBy(p => p.Y).ToArray()))
            .ToDictionary(p => p.Item1, p => new Room(p.Item1, p.Item2.First().X, p.Item2));

        return rooms;
    }

    private static Hallway GetHallway(Map<char> map)
    {
        var hallwayPoints = map.Where((p, v) => v is '.' && map.GetValue(p.X, p.Y + 1) == '#').OrderBy(p => p.X);
        return new(hallwayPoints.ToArray());
    }

    private async Task<Map<char>> GetMapPart2()
    {
        var parts = FilePath.Split('.');
        var file = string.Join('.', parts.SkipLast(1)) + "-2." + parts.Last();
        return await GetMap(file);
    }

    private static async Task<Map<char>> GetMap(string filePath) =>
        new((await File.ReadAllLinesAsync(filePath))
            .Where(l => l.IsNotNullOrEmpty())
            .Select(l => l.ToCharArray().ToArray())
            .ToArray());

    private class State : IEquatable<State>
    {
        private int _hashCode;
        private readonly HashSet<Amphipod> _amphipods = new();

        public State(IEnumerable<Amphipod> amphipods)
        {
            foreach (var amphipod in amphipods)
            {
                _amphipods.Add(amphipod);
            }
        }

        public State(IEnumerable<Amphipod> amphipods, Amphipod oldLocation, Amphipod newLocation)
        {
            foreach (var amphipod in amphipods)
            {
                if (amphipod.Equals(oldLocation))
                {
                    _amphipods.Add(newLocation);
                }
                else
                {
                    _amphipods.Add(amphipod);
                }
            }
        }

        public IReadOnlySet<Amphipod> Amphipods => _amphipods;
        public int Cost { get; init; }

        public bool Equals(State? other) =>
            other is not null &&
            other.Cost == Cost &&
            other.Amphipods.Count == _amphipods.Count &&
            other.Amphipods.All(a => _amphipods.Contains(a));

        public override bool Equals(object? obj) =>
            Equals(obj as State);

        public override int GetHashCode()
        {
            if (_hashCode is 0)
            {
                _hashCode = 19;
                var amphipods = _amphipods.OrderBy(a => a.Type * 1000 + a.Location.X * 10 + a.Location.Y);
                foreach (var amphipod in amphipods)
                {
                    _hashCode = _hashCode * 31 + amphipod.Type.GetHashCode();
                    _hashCode = _hashCode * 31 + amphipod.Location.GetHashCode();
                }
            }

            return _hashCode;
        }
    }

    private class Room
    {
        public char Type { get; init; }
        public int X { get; init; }
        public Point[] Points { get; init; } = Array.Empty<Point>();

        public Room(char type, int x, Point[] points)
        {
            Type = type;
            X = x;
            Points = points;
        }

        public bool IsRoomFinished(State state) =>
            Points.All(p => state.Amphipods.Any(a => a.Type == Type && a.Location == p));

        public bool CanMoveIntoRoom(State state) =>
            state.Amphipods.Where(a => Points.Contains(a.Location)).All(a => a.Type == Type);

        public Point GetNextFreePoint(State state) =>
            Points.LastOrDefault(p => state.Amphipods.All(a => a.Location != p));

        public bool IsAmphipodFinished(State state, Amphipod amphipod) =>
            amphipod.Type == Type &&
            Points.Any(p => p == amphipod.Location) &&
            Points.Where(p => p.Y > amphipod.Location.Y).All(p => state.Amphipods.Any(a => a.Type == Type && a.Location == p));
    }

    private record struct Amphipod(char Type, Point Location);
    private record struct Hallway(Point[] Points)
    {
        public int Left => Points.FirstOrDefault().X;
        public int Right => Points.LastOrDefault().X;
        public int Y => Points.FirstOrDefault().Y;
    };
    private record Burrow(Dictionary<char, Room> Rooms, Hallway Hallway, Dictionary<char, int> TypeCost);
}