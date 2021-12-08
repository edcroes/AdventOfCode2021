namespace AoC2021.Day04;

public class Day04 : IMDay
{
    public string FilePath { private get; init; } = "Day04\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var numbersToCall = await ParseNumbers();
        var bingoCards = await ParseBingoCards();

        int score = 0;
        foreach(var number in numbersToCall)
        {
            foreach (var card in bingoCards)
            {
                card.MarkNumber(number);
            }

            var winner = bingoCards.FirstOrDefault(c => c.HasBingo);
            if (winner is not null)
            {
                score = winner.SumOfUnmarkedNumbers * number;
                break;
            }
        }

        return score.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var numbersToCall = await ParseNumbers();
        var bingoCards = await ParseBingoCards();
        
        int score = 0;
        BingoCard? lastCard = null;
        foreach (var number in numbersToCall)
        {
            foreach (var card in bingoCards)
            {
                card.MarkNumber(number);
            }

            if (lastCard is not null && lastCard.HasBingo)
            {
                score = lastCard.SumOfUnmarkedNumbers * number;
                break;
            }

            bingoCards = bingoCards.Where(c => !c.HasBingo).ToArray();
            if (bingoCards.Length == 1)
            {
                lastCard = bingoCards[0];
            }
        }

        return score.ToString();
    }

    private async Task<IEnumerable<int>> ParseNumbers() =>
        (await File.ReadAllLinesAsync(FilePath))
            .First()
            .Split(',')
            .Select(n => int.Parse(n));

    private async Task<BingoCard[]> ParseBingoCards() =>
        (await File.ReadAllTextAsync(FilePath))
            .Replace("\r\n", "\n")
            .Split("\n\n")
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Skip(1)
            .Select(s => BingoCard.Parse(s.Split("\n").Where(l => !string.IsNullOrWhiteSpace(l)).ToArray()))
            .ToArray();
}