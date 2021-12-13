namespace AoC2021.Day12;

public class Day12 : IMDay
{
    private record Link(string From, string To);

    public string FilePath { private get; init; } = "Day12\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var links = await GetLinks();
        List<List<string>> paths = new() { new List<string> { "start" } };

        while (paths.Any(p => !HasEnded(p)))
        {
            foreach (var path in paths.Where(p => !HasEnded(p)).ToArray())
            {
                List<List<string>> newPaths = new();
                foreach (var nextCave in links[path.Last()])
                {
                    if (IsPathAllowed(path, nextCave))
                    {
                        newPaths.Add(new(path) { nextCave });
                    }
                }

                paths.Remove(path);
                paths.AddRange(newPaths);
            }
        }

        return paths.Count.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var links = await GetLinks();
        List<string> paths = new() { "-start" };

        while (paths.Any(p => !HasEnded(p)))
        {
            foreach (var path in paths.Where(p => !HasEnded(p)).ToArray())
            {
                List<string> newPaths = new();
                var lastCave = path.Substring(path.LastIndexOf('-') + 1);

                foreach (var nextCave in links[lastCave])
                {
                    if (IsPathAllowed(path, nextCave))
                    {
                        var newPath = string.Empty;
                        if (nextCave == nextCave.ToLower() && path.Contains($"-{nextCave}-"))
                        {
                            newPath = "1";
                        }

                        newPath += path + "-" + nextCave;
                        newPaths.Add(newPath);
                    }
                }

                paths.Remove(path);
                paths.AddRange(newPaths);
            }
        }

        return paths.Count.ToString();
    }

    private static bool HasEnded(List<string> path) => path.Last() == "end";

    private static bool HasEnded(string path) => path.EndsWith("-end");

    private static bool IsPathAllowed(List<string> path, string nextCave) =>
        nextCave == nextCave.ToUpper() || !path.Contains(nextCave);

    private static bool IsPathAllowed(string path, string nextCave) =>
        nextCave == nextCave.ToUpper() || path[0] != '1' || !path.Contains($"-{nextCave}-");

    private async Task<Dictionary<string, List<string>>> GetLinks()
    {
        var rawLinks = (await File.ReadAllLinesAsync(FilePath))
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => l.Split('-'));
        Dictionary<string, List<string>> links = new();

        foreach (var link in rawLinks)
        {
            if (link[1] != "start" && link[0] != "end")
            {
                AddOrUpdate(links, link[0], link[1]);
            }

            if (link[0] != "start" && link[1] != "end")
            {
                AddOrUpdate(links, link[1], link[0]);
            }
        }

        return links;
    }

    private static void AddOrUpdate(Dictionary<string, List<string>> dictionary, string key, string value)
    {
        if (!dictionary.ContainsKey(key))
        {
            dictionary.Add(key, new());
        }

        if (!dictionary[key].Contains(value))
        {
            dictionary[key].Add(value);
        }
    }
}