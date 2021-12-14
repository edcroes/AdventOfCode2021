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

/*
    Template:     NNCB
    After step 1: NCNBCHB
    After step 2: NBCCNBBBCBHCB

    In pairs that is:
    Template:     NN NC CB
    After step 1: NC CN NB BC CH HB
    After step 2: NB BC CC CN NB BB BB BC CB BH HC CB

    NN translates to NCN which is NC CN in pairs (rule is NN -> C)
    So each pair is 2 pairs in the next step.

    First try:
    - for each step, get the new pairs for each pair
    - Is becoming slow around step 20
    - Reason for this is that for every step it has to process twice as much pairs

    Second try:
    - for each step, get the unique pairs and count how many times they appear, for each unique pair get the new pairs and count those
    - Because we're only iterating unique pairs this is way faster

    Now that we have a result with the occurrences per pair, we can calculate the occurrences of a character.
    We only need the first character of each pair since the last char is always the first char in another pair.
    We shouldn't forget the last character of the template now since that is never the first character of a pair.
*/