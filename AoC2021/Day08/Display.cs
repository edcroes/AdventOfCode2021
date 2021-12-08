namespace AoC2021.Day08;

public class Display
{
    private readonly string[] _wires;
    private readonly char[][] _allDigits;

    public string[] Digits { get; init; }

    public Display(string[] wires, string[] digits)
    {
        _wires = wires;
        Digits = digits;
        _allDigits = Decode();
    }

    public int DisplayedNumber =>
            Enumerable.Range(0, Digits.Length)
                .Select(i => GetNumber(Digits[i]) * (int)Math.Pow(10, Digits.Length - (i + 1)))
                .Sum();

    private int GetNumber(string digit)
    {
        var chars = digit.ToCharArray();
        var foundDigit = _allDigits.Single(d => d.Length == chars.Length && chars.Intersect(d).Count() == chars.Length);
        return Array.IndexOf(_allDigits, foundDigit);
    }

    private char[][] Decode()
    {
        var one = _wires.Single(w => w.Length == 2).ToCharArray();
        var three = _wires.Single(w => w.Length == 5 && one.Intersect(w.ToCharArray()).Count() == one.Length).ToCharArray();
        var four = _wires.Single(w => w.Length == 4).ToCharArray();
        var six = _wires.Single(w => w.Length == 6 && one.Intersect(w.ToCharArray()).Count() != one.Length).ToCharArray();
        var five = _wires.Single(w => w.Length == 5 && w.ToCharArray().Intersect(six).Count() == w.ToCharArray().Length).ToCharArray();
        var seven = _wires.Single(w => w.Length == 3).ToCharArray();
        var eight = _wires.Single(w => w.Length == 7).ToCharArray();
        var nine = _wires.Single(w => w.Length == 6 && w.ToCharArray().Intersect(three).Count() == three.Length).ToCharArray();
        var two = _wires.Single(w => w.Length == 5 && w.ToCharArray().Intersect(nine).Count() == 4).ToCharArray();
        var zero = _wires.Single(w => w.Length == 6 && w.ToCharArray().Intersect(five).Count() == 4).ToCharArray();

        return new char[][]
        {
            zero,
            one,
            two,
            three,
            four,
            five,
            six,
            seven,
            eight,
            nine
        };
    }
}
