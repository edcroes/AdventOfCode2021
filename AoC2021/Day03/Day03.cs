﻿namespace AoC2021.Day03;

public class Day03 : IMDay
{
    private readonly string _file;

    public Day03(string file)
    {
        _file = file;
    }

    public Day03() : this("Day03\\input.txt") { }

    public async Task<string> GetAnswerPart1()
    {
        var numbers = await GetNumbers();
        var gammaRate = string.Empty;
        var epsilonRate = string.Empty;

        for (var i = 0; i < numbers[0].Length; i++)
        {
            var mostCommonBit = GetMostCommonBit(numbers, i);
            gammaRate += mostCommonBit ? "1" : "0";
            epsilonRate += mostCommonBit ? "0" : "1";
        }

        var result = Convert.ToInt32(gammaRate, 2) * Convert.ToInt32(epsilonRate, 2);
        return result.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var numbers = await GetNumbers();
        Console.WriteLine($"Oxy {GetOxygenGeneratorRating(numbers)}");
        Console.WriteLine($"CO2 {GetCO2ScrubberRating(numbers)}");
        var result = GetOxygenGeneratorRating(numbers) * GetCO2ScrubberRating(numbers);

        return result.ToString();
    }

    private static int GetOxygenGeneratorRating(string[] numbers)
    {
        var remainingNumbers = numbers.ToArray();
        for (var i = 0; i < numbers[0].Length; i++)
        {
            var mostCommonBit = GetMostCommonBit(remainingNumbers, i) ? '1' : '0';
            remainingNumbers = remainingNumbers.Where(n => n.ToCharArray()[i] == mostCommonBit).ToArray();
            Console.WriteLine($"\r\nRemaining after {i}");
            foreach (var r in remainingNumbers)
            {
                Console.WriteLine($"\t{r}");
            }
            if (remainingNumbers.Length == 1)
            {
                return Convert.ToInt16(remainingNumbers[0], 2);
            }
        }

        return 0;
    }

    private static int GetCO2ScrubberRating(string[] numbers)
    {
        var remainingNumbers = numbers.ToArray();
        for (var i = 0; i < numbers[0].Length; i++)
        {
            var leastCommonBit = GetMostCommonBit(remainingNumbers, i) ? '0' : '1';
            remainingNumbers = remainingNumbers.Where(n => n.ToCharArray()[i] == leastCommonBit).ToArray();

            if (remainingNumbers.Length == 1)
            {
                return Convert.ToInt16(remainingNumbers[0], 2);
            }
        }

        return 0;
    }

    private static bool GetMostCommonBit(string[] values, int atPosition)
    {
        var totalOnes = values
            .Select(n => n.ToCharArray()[atPosition])
            .Count(b => b == '1');

        return totalOnes >= (double)values.Length / 2;
    }

    private async Task<string[]> GetNumbers() =>
        (await File.ReadAllLinesAsync(_file))
            .Where(l => !string.IsNullOrEmpty(l))
            .ToArray();
}