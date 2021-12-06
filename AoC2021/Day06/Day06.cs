namespace AoC2021.Day06;

public class Day06 : IMDay
{
    private const int PregnancyPeriod = 7;
    private const int FirstPregnancyExtraPeriod = 2;
    private readonly string _fishFile;

    public Day06(string fishFile)
    {
        _fishFile = fishFile;
    }

    public Day06() : this("Day06\\input.txt") { }

    public async Task<string> GetAnswerPart1()
    {
        var result = await CalculateNumberOfFishAtDay(80);
        return result.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var result = await CalculateNumberOfFishAtDay(256);
        return result.ToString();
    }

    private async Task<long> CalculateNumberOfFishAtDay(int day)
    {
        var initialFish = await GetFish();
        var newFishPerDay = Enumerable.Range(0, day)
                                .Select(day => (long)initialFish.Count(f => f == day % PregnancyPeriod))
                                .ToArray();

        for (var i = 0; i < day; i++)
        {
            if (newFishPerDay[i] == 0)
            {
                continue;
            }

            var nextTimeBirth = i + PregnancyPeriod + FirstPregnancyExtraPeriod;
            while (nextTimeBirth < day)
            {
                newFishPerDay[nextTimeBirth] += newFishPerDay[i];
                nextTimeBirth += PregnancyPeriod;
            }
        }

        return newFishPerDay.Sum(f => f) + initialFish.Count;
    }

    private async Task<List<int>> GetFish() =>
        (await File.ReadAllTextAsync(_fishFile))
            .Split(',')
            .Select(l => int.Parse(l))
            .ToList();
}