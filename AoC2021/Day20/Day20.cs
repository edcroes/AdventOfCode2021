namespace AoC2021.Day20;

public class Day20 : IMDay
{
    public string FilePath { private get; init; } = "Day20\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var (enhancementAlgo, image) = await ParseImageInfo();

        image = EnhanceImage(enhancementAlgo, image, 0);
        image = EnhanceImage(enhancementAlgo, image, 1);

        return image.Count('#').ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var (enhancementAlgo, image) = await ParseImageInfo();

        for (var i = 0; i < 50; i++)
        {
            image = EnhanceImage(enhancementAlgo, image, i);
        }

        return image.Count('#').ToString();
    }

    private static Map<char> EnhanceImage(char[] algo, Map<char> image, int round)
    {
        // If the 000000000 in the algo is # it means that each round it flips from 000000000 -> 111111111
        var background = algo[0] == '#' && round % 2 == 0
            ? algo[^1]
            : algo[0];

        var newImage = image.EnlargeMapByOneOnEachSide(background);
        var imageSource = newImage.Clone();

        newImage.ForEach((p, v) =>
        {
            var algoIndex = GetIndexFromPoint(imageSource, p, background);
            newImage.SetValue(new Point(p.X, p.Y), algo[algoIndex]);
        });

        return newImage;
    }

    private static short GetIndexFromPoint(Map<char> image, Point point, char defaultBackground)
    {
        var binary = string.Empty;

        for (int y = point.Y - 1; y <= point.Y + 1; y++)
        {
            for (int x = point.X - 1; x <= point.X + 1; x++)
            {
                binary += image.GetValueOrDefault(x, y, defaultBackground) == '#' ? "1" : "0";
            }
        }

        return Convert.ToInt16(binary, 2);
    }

    private async Task<(char[], Map<char>)> ParseImageInfo()
    {
        var fileParts = (await File.ReadAllTextAsync(FilePath))
            .Replace("\r\n", "\n")
            .Split("\n\n");

        Map<char> map = new(fileParts[1]
            .Split('\n')
            .Where(l => l.Length > 0)
            .Select(l => l.ToCharArray())
            .ToArray());

        return (fileParts[0].ToCharArray(), map);
    }
}