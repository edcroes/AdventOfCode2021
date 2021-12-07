namespace AoC2021.Day07;

public class Day07 : IMDay
{
    private readonly string _crabFile;

    public Day07(string crabFile)
    {
        _crabFile = crabFile;
    }

    public Day07() : this("Day07\\input.txt") { }

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
            var totalFuelCost = 0;
            foreach (var position in crabPositions)
            {
                int steps = Math.Abs(position - moveTo);
                totalFuelCost += fuelCalculation(steps);
            }
            leastFuelUsage = Math.Min(leastFuelUsage, totalFuelCost);
        }

        return leastFuelUsage;
    }

    private async Task<int[]> GetCrabPositions() =>
        (await File.ReadAllTextAsync(_crabFile))
            .Split(',')
            .Select(l => int.Parse(l))
            .OrderBy(l => l)
            .ToArray();
}