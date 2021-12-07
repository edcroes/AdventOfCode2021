using AoC2021.Common.Maps;

namespace AoC2021.Day04
{
    public class BingoCard
    {
        private readonly Map<int> _card;

        private BingoCard(Map<int> card)
        {
            _card = card;
        }

        public void MarkNumber(int number)
        {
            for (var row = 0; row < _card.SizeY; row++)
            {
                for (var column = 0; column < _card.SizeX; column++)
                {
                    if (_card.GetValue(column, row) == number)
                    {
                        _card.SetValue(column, row, -1);
                        return;
                    }
                }
            }
        }

        public bool HasBingo
        {
            get
            {
                for (var row = 0; row < _card.SizeY; row++)
                {
                    if (_card.GetLine(0, row, _card.SizeX - 1, row).All(n => n == -1))
                    {
                        return true;
                    }
                }

                for (var column = 0; column < _card.SizeX; column++)
                {
                    if (_card.GetLine(column, 0, column, _card.SizeY - 1).All(n => n == -1))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public int SumOfUnmarkedNumbers => _card.ToFlatArray().Where(n => n != -1).Sum();

        public static BingoCard Parse(string[] lines)
        {
            Map<int> card = new(lines.Length, lines.Length);

            for (var row = 0; row < lines.Length; row++)
            {
                var numbers = ParseLine(lines, row);
                if (numbers.Length != lines.Length)
                {
                    throw new ArgumentException("A Bingo card should be square");
                }

                for (var column = 0; column < numbers.Length; column++)
                {
                    card.SetValue(column, row, numbers[column]);
                }
            }

            return new BingoCard(card);
        }

        private static int[] ParseLine(string[] lines, int row) =>
            lines[row]
                .Trim()
                .Replace("  ", " ")
                .Split(" ")
                .Select(n => int.Parse(n))
                .ToArray();
    }
}
