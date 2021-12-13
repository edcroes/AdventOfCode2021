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
                var pathLinks = links.Where(l => l.From == path.Last() || l.To == path.Last());
                foreach (var link in pathLinks)
                {
                    var nextCave = link.From == path.Last() ? link.To : link.From;
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
                var pathLinks = links.Where(l => path.EndsWith($"-{l.From}") || path.EndsWith($"-{l.To}"));
                foreach (var link in pathLinks)
                {
                    var nextCave = path.EndsWith($"-{link.To}") ? link.From : link.To;
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
        nextCave != "start" && (nextCave == nextCave.ToUpper() || !path.Contains(nextCave));

    private static bool IsPathAllowed(string path, string nextCave) =>
        nextCave != "start" && (nextCave == nextCave.ToUpper() || path[0] != '1' || !path.Contains($"-{nextCave}-"));

    private async Task<IEnumerable<Link>> GetLinks() =>
        (await File.ReadAllLinesAsync(FilePath))
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => l.Split('-'))
            .Select(l => new Link(l[0], l[1]));
}