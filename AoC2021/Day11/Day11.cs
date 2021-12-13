namespace AoC2021.Day11;

public class Day11 : IMDay
{
    public string FilePath { private get; init; } = "Day11\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var octopuses = await GetOctopuses();
        var totalFlashes = 0;

        for (var step = 0; step < 100; step++)
        {
            Console.WriteLine($"Step {step}");
            totalFlashes += FlashOctopuses(octopuses);
        }

        return totalFlashes.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var octopuses = await GetOctopuses();
        var currentStep = 0;

        while(!octopuses.All((p, v) => v == 0))
        {
            currentStep++;
            _ = FlashOctopuses(octopuses);
        }

        return currentStep.ToString();
    }
    private static int FlashOctopuses(Map<int> octopuses)
    {
        List<Point> flashingPoints = new();
        octopuses.ForEach((point, value) => octopuses.SetValue(point, value + 1));

        var currentFlashingPoints = octopuses.Where((p, v) => v > 9);
        while (currentFlashingPoints.Any())
        {
            flashingPoints.AddRange(currentFlashingPoints);
            foreach (var point in currentFlashingPoints)
            {
                var neighbors = octopuses.GetStraightAndDiagonalNeighbors(point);
                foreach (var neighbor in neighbors)
                {
                    octopuses.SetValue(neighbor, octopuses.GetValue(neighbor) + 1);
                }
            }

            currentFlashingPoints = octopuses.Where((p, v) => v > 9 && !flashingPoints.Contains(p));
        }

        flashingPoints.ForEach(p => octopuses.SetValue(p, 0));

        return flashingPoints.Count;
    }

    private async Task<Map<int>> GetOctopuses() =>
        new((await File.ReadAllLinesAsync(FilePath))
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => l.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray())
            .ToArray());
}
