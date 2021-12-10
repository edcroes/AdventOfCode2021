namespace AoC2021.Day02;

public class Day02 : IMDay
{
    private record Movement(string Direction, int Steps);

    public string FilePath { private get; init; } = "Day02\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var movements = await GetMovements();
        var result = movements
            .Select(m => m.Direction switch
            {
                "up" => new Point(0, -1 * m.Steps),
                "down" => new Point(0, m.Steps),
                "forward" => new Point(m.Steps, 0),
                _ => throw new InvalidOperationException($"I can't do anything with '{m.Direction}'")
            })
            .Aggregate((first, second) => new Point(first.X + second.X, first.Y + second.Y));

        return (result.X * result.Y).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var movements = await GetMovements();
        var aim = 0;
        var position = new Point(0, 0);

        foreach (var movement in movements)
        {
            switch (movement.Direction)
            {
                case "up":
                    aim -= movement.Steps;
                    break;
                case "down":
                    aim += movement.Steps;
                    break;
                case "forward":
                    position = new Point(position.X + movement.Steps, position.Y + movement.Steps * aim);
                    break;
                default:
                    throw new InvalidOperationException($"I can't do anything with '{movement.Direction}'");
            }
        }

        return ((long)position.X * position.Y).ToString();
    }

    private async Task<IEnumerable<Movement>> GetMovements() =>
        (await File.ReadAllLinesAsync(FilePath))
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => l.Split(" "))
            .Select(m => new Movement(m[0], int.Parse(m[1])));
}
