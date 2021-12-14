namespace AoC2021.Day14;

public class Day14 : IMDay
{
    private Dictionary<string, string[]> _rules;
    private string _template;

    public string FilePath { private get; init; } = "Day14\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        await Init();
        var templatePairs = GetPairs(_template);

        for (var step = 0; step < 10; step++)
        {
            templatePairs = CalculateNextPairs(templatePairs);
        }

        var letterCount = CalculateLetterCount(templatePairs);
        var result = letterCount.Values.Max() - letterCount.Values.Min();

        return result.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        await Init();
        var templatePairs = GetPairs(_template);

        for (var step = 0; step < 40; step++)
        {
            templatePairs = CalculateNextPairs(templatePairs);
        }

        var letterCount = CalculateLetterCount(templatePairs);
        var result = letterCount.Values.Max() - letterCount.Values.Min();

        return result.ToString();
    }

    private static Dictionary<string, long> GetPairs(string template) =>
        Enumerable.Range(0, template.Length - 1)
            .Select(i => template[i].ToString() + template[i + 1])
            .GroupBy(t => t)
            .ToDictionary(g => g.Key, g => (long)g.Count());

    private Dictionary<string, long> CalculateNextPairs(Dictionary<string, long> templatePairs)
    {
        Dictionary<string, long> newPairs = new();
        foreach (var pair in templatePairs.Keys)
        {
            foreach (var newPair in _rules[pair])
            {
                newPairs.AddOrUpdate(newPair, templatePairs[pair]);
            }
        }

        return newPairs;
    }

    private Dictionary<char, long> CalculateLetterCount(Dictionary<string, long> templatePairs)
    {
        Dictionary<char, long> letterCount = new();
        foreach (var key in templatePairs.Keys)
        {
            letterCount.AddOrUpdate(key[0], templatePairs[key]);
        }

        letterCount.AddOrUpdate(_template[^1], 1);
        return letterCount;
    }

    private async Task Init()
    {
        if (_template is not null && _rules is not null)
        {
            return;
        }

        var lines = await File.ReadAllLinesAsync(FilePath);
        _template = lines.First();
        _rules = lines
            .Skip(2)
            .Where(l => !string.IsNullOrEmpty(l))
            .Select(l => l.Split(" -> "))
            .Select(l => new KeyValuePair<string, string[]>(l[0], new[] { l[0][0] + l[1], l[1] + l[0][1] }))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}