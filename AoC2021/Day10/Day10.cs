namespace AoC2021.Day10;

public class Day10 : IMDay
{
    private readonly Dictionary<char, char> _pairs = new()
    {
        { '(', ')' },
        { '[', ']' },
        { '{', '}' },
        { '<', '>' }
    };

    private readonly Dictionary<char, int> _corruptionScores = new()
    {
        { ')', 3 },
        { ']', 57 },
        { '}', 1197 },
        { '>', 25137 }
    };

    private readonly Dictionary<char, int> _incompleteScores = new()
    {
        { '(', 1 },
        { '[', 2 },
        { '{', 3 },
        { '<', 4 }
    };

    public string FilePath { private get; init; } = "Day10\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var subsystem = await GetSubsystem();
        var score = subsystem
            .Select(l => GetCorruptionScore(l))
            .Sum();

        return score.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var subsystem = await GetSubsystem();

        var scores = subsystem
            .Where(l => GetCorruptionScore(l) == 0)
            .Select(l => GetIncompleteScore(l))
            .Where(s => s > 0)
            .OrderBy(s => s)
            .ToList();
            
        var result = scores
            .Skip(scores.Count / 2)
            .Take(1)
            .Single();

        return result.ToString();
    }

    private int GetCorruptionScore(char[] line)
    {
        Stack<char> open = new();
        var score = 0;

        foreach (var c in line)
        {
            if (IsOpen(c))
            {
                open.Push(c);
            }
            else if (_pairs[open.Pop()] != c)
            {
                score = _corruptionScores[c];
                break;
            }
        }

        return score;
    }

    private long GetIncompleteScore(char[] line)
    {
        Stack<char> open = new();

        foreach (var c in line)
        {
            if (IsOpen(c))
            {
                open.Push(c);
            }
            else
            {
                _ = open.Pop();
            }
        }

        var score = 0L;
        while (open.Count > 0)
        {
            score = score * 5 + _incompleteScores[open.Pop()];
        }

        return score;
    }

    private bool IsOpen(char c) => _pairs.ContainsKey(c);

    private async Task<char[][]> GetSubsystem() =>
        (await File.ReadAllLinesAsync(FilePath))
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => l.ToCharArray())
            .ToArray();
}
