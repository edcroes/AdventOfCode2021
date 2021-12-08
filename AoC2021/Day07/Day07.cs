namespace AoC2021.Day07;

public class Day07 : IMDay
{
    public string FilePath { private get; init; } = "Day07\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var result = await GetLeastFuelUsage(s => s);
        return result.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        static int fuelCalculation(int steps) => (int)(steps * ((double)steps / 2 + .5));
        var result = await GetLeastFuelUsage(fuelCalculation);

        return result.ToString();
    }

    private async Task<int> GetLeastFuelUsage(Func<int, int> fuelCalculation)
    {
        var crabPositions = await GetCrabPositions();
        var leastFuelUsage = int.MaxValue;

        for (var moveTo = crabPositions.Min(); moveTo <= crabPositions.Max(); moveTo++)
        {
            var totalFuelCost = crabPositions
                .Select(p => fuelCalculation(Math.Abs(p - moveTo)))
                .Sum();
            leastFuelUsage = Math.Min(leastFuelUsage, totalFuelCost);
        }

        return leastFuelUsage;
    }

    private async Task<int[]> GetCrabPositions() =>
        (await File.ReadAllTextAsync(FilePath))
            .Split(',')
            .Select(l => int.Parse(l))
            .OrderBy(l => l)
            .ToArray();
}