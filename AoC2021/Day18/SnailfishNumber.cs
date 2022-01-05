namespace AoC2021.Day18;

public class SnailfishNumber
{
    private const int Open = -1;
    private const int Close = -2;

    private List<int> Number { get; } = new();

    private SnailfishNumber(SnailfishNumber cloneFrom)
    {
        Number.AddRange(cloneFrom.Number);
    }

    public SnailfishNumber(string number)
    {
        foreach (var chr in number)
        {
            var value = chr switch
            {
                '[' => Open,
                ']' => Close,
                ',' => int.MinValue,
                _ => chr - 48
            };

            if (value != int.MinValue)
            {
                Number.Add(value);
            }
        }
    }

    public string Value
    {
        get
        {
            var parts = Enumerable.Range(0, Number.Count)
                .Select(i => Number[i] switch
                {
                    Open => "[",
                    Close when i + 1 >= Number.Count || Number[i + 1] == Close => "]",
                    Close => "],",
                    _ when Number[i + 1] != Close => $"{Number[i]},",
                    _ => Number[i].ToString()
                });

            return string.Join(string.Empty, parts);
        }
    }

    public int Magnitude
    {
        get
        {
            List<int> magnitude = new(Number);

            while (magnitude.Count > 1)
            {
                var indexFirstSimplePair = Enumerable.Range(0, magnitude.Count - 1)
                    .FirstOrDefault(i => magnitude[i] >= 0 && magnitude[i + 1] >= 0);
                var pairMagnitude = magnitude[indexFirstSimplePair] * 3 + magnitude[indexFirstSimplePair + 1] * 2;
                
                magnitude.RemoveRange(indexFirstSimplePair - 1, 4);
                magnitude.Insert(indexFirstSimplePair - 1, pairMagnitude);
            }

            return magnitude.SingleOrDefault();
        }
    }

    public override string ToString() => Value;

    public static SnailfishNumber operator +(SnailfishNumber first, SnailfishNumber second)
    {
        SnailfishNumber newNumber = new(first);
        newNumber.Add(second);
        return newNumber;
    }

    public void Add(SnailfishNumber other)
    {
        Number.Insert(0, Open);
        Number.AddRange(other.Number);
        Number.Add(Close);

        while (TryExplode() || TrySplit());
    }

    private bool TryExplode()
    {
        var depth = 0;
        var lastLeftNumberIndex = -1;

        for (var i = 0; i < Number.Count; i++)
        {
            if (Number[i] < 0)
            {
                depth += Number[i] == Open ? 1 : -1;
            }
            else if (depth > 4)
            {
                UpdateLeftNumberAfterExplosion(lastLeftNumberIndex, i);
                UpdateRightNumberAfterExplosion(i + 1);

                Number.RemoveRange(i - 1, 4);
                Number.Insert(i - 1, 0);

                return true;
            }
            else
            {
                lastLeftNumberIndex = i;
            }
        }

        return false;
    }

    private void UpdateLeftNumberAfterExplosion(int leftNumberIndex, int currentNumberIndex)
    {
        if (leftNumberIndex >= 0)
        {
            Number[leftNumberIndex] += Number[currentNumberIndex];
        }
    }

    private void UpdateRightNumberAfterExplosion(int currentNumberIndex)
    {
        var rightNumberIndex = Enumerable.Range(currentNumberIndex + 1, Number.Count - currentNumberIndex - 2)
            .FirstOrDefault(i => Number[i] >= 0);

        if (rightNumberIndex != default)
        {
            Number[rightNumberIndex] += Number[currentNumberIndex];
        }
    }

    private bool TrySplit()
    {
        var numberToSplit = Number.FirstOrDefault(n => n > 9);
        if (numberToSplit == default)
        {
            return false;
        }

        var index = Number.IndexOf(numberToSplit);
        Number.RemoveAt(index);
        Number.InsertRange(index, new[]
        {
            Open,
            numberToSplit / 2,
            (numberToSplit + 1) / 2,
            Close
        });

        return true;
    }
}