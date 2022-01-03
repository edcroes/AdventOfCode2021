namespace AoC2021.Day24;

public class Day24 : IMDay
{
    private record struct Input(bool Push, int Var1, int Var2, int Index);

    public string FilePath { private get; init; } = "Day24\\input.txt";   

    public async Task<string> GetAnswerPart1()
    {
        var instructions = await GetProgram();
        var combinations = GetInputCombinations(instructions);

        var largestNumber = 0L;
        
        foreach (var combi in combinations)
        {
            var leftMaxW = Math.Min(9, 9 - combi.Item1.Var2 - combi.Item2.Var1);
            var rightW = leftMaxW + combi.Item1.Var2 + combi.Item2.Var1;
            largestNumber = largestNumber.SetNthDigit(13 - combi.Item1.Index, leftMaxW);
            largestNumber = largestNumber.SetNthDigit(13 - combi.Item2.Index, rightW);
        }

        if (!CheckLicense(largestNumber, instructions))
        {
            throw new InvalidOperationException("Calculation of the largest number has failed big time");
        }

        return largestNumber.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var instructions = await GetProgram();
        var combinations = GetInputCombinations(instructions);

        var smallestNumber = 0L;

        foreach (var combi in combinations)
        {
            var leftMinW = Math.Max(1, 1 - combi.Item1.Var2 - combi.Item2.Var1);
            var rightW = leftMinW + combi.Item1.Var2 + combi.Item2.Var1;
            smallestNumber = smallestNumber.SetNthDigit(13 - combi.Item1.Index, leftMinW);
            smallestNumber = smallestNumber.SetNthDigit(13 - combi.Item2.Index, rightW);
        }

        if (!CheckLicense(smallestNumber, instructions))
        {
            throw new InvalidOperationException("Calculation of the smallest number has failed big time");
        }

        return smallestNumber.ToString();
    }

    private static List<(Input, Input)> GetInputCombinations(AluInstruction[] instructions)
    {
        var inputIndex = 0;
        var parts = instructions
            .Split(new AluInstruction("inp", 'w', string.Empty))
            .Select(p => new Input(
                GetDivideZBy(p) == 1,
                GetVariable1(p),
                GetVariable2(p),
                inputIndex++))
            .ToArray();

        List<(Input, Input)> inputCombinations = new();
        Stack<Input> pushes = new();
        foreach (var part in parts)
        {
            if (part.Push)
            {
                pushes.Push(part);
            }
            else
            {
                inputCombinations.Add((pushes.Pop(), part));
            }
        }

        return inputCombinations;
    }

    private static int GetDivideZBy(AluInstruction[] instructions) =>
        int.Parse(instructions.Single(i => i.Instruction == "div" && i.Left == 'z').Right);

    private static int GetVariable1(AluInstruction[] instructions) =>
        int.Parse(instructions.First(i => i.Instruction == "add" && i.Left == 'x' && i.Right != "z").Right);

    private static int GetVariable2(AluInstruction[] instructions) =>
        int.Parse(instructions.Last(i => i.Instruction == "add" && i.Left == 'y').Right);

    private static bool CheckLicense(long numberToCheck, AluInstruction[] instructions)
    {
        Alu alu = new();
        Queue<int> input = new();

        for (var n = 13; n >= 0; n--)
        {
            input.Enqueue(numberToCheck.GetNthDigit(n));
        }

        alu.Execute(instructions, () => input.Dequeue());
        return alu.Registers['z'] == 0;
    }

    private async Task<AluInstruction[]> GetProgram() =>
        (await File.ReadAllLinesAsync(FilePath))
            .Where(l => l.IsNotNullOrEmpty())
            .Select(l => l.Split(' '))
            .Select(i => new AluInstruction(
                i[0],
                i[1][0],
                (i.Length > 2) ? i[2] : string.Empty
            ))
            .ToArray();
}

/*
Looking at the assembly we see a pattern that is repeated for every input:
inp w
mul x 0
add x z
mod x 26
div z 1     <- z is divided by 1 or 26
add x 10    <- 10 is different for each input, var1
eql x w
eql x 0
mul y 0
add y 25
mul y x
add y 1
mul z y
mul y 0
add y w
add y 1     <- 1 is different for each input, var2
mul y x
add z y

This results in the simplified function:
private int ProcessInput(int z, int w, int divideZBy, int var1, int var2)
{
    var x = z % 26 + var1;
    z /= divideZBy;

    if (x != w)
    {
        z = z * 26 + w + var2;
    }

    return z;
}

Looking at the variables we can make some assumptions that are at least true for my input:
1. When z is divided by 1 (nothing happens), var1 is always higher than 9
2. Because of 1 x will never equal y when z is divided by 1
3. Because of 2, z will always be raised when z is divided by 1
4. var2 is never higher than 16 which makes that w + var2 is always lower than 26
5. z is divided by 1 7 times and divided by 26 7 times, so if dividing by 1 is raising z then dividing by 26 should lower z
6. Because of 6, when z is divided by 26 then x must equal w to lower z

Taking all these assumptions to the test learns that we're dealing with a stack here where z is the stack.
Dividing z by 1 means we're pushing to the stack, dividing z by 26 means we're popping from the stack.
When we're pushing to the stack, the current z is first multiplied with 26, which means that the current value of z is not taken into account
for the next assignment of x, z % 26 on this part of z is always 0. Because of assumption 4 we know that the new value that is added to z is
always lower than 26 so it won't interfere with previous values of z.
Dividing z by 26 pops the last value from the stack, because the last value of z was lower than 26 and so will nog longer be part of z.

A small example with 6 inputs:
+----------+---+---+---+---+---+---+
| Div z by |  1|  1|  1| 26| 26| 26|
+----------+---+---+---+---+---+---+
| var1     | 11| 13|  9| -3|-11|-14|
+----------+---+---+---+---+---+---+
| var2     | 12|  4|  8|  3|  7| 15|
+----------+---+---+---+---+---+---+

We begin with z = 0, nothing on the stack.

After input 1 (push)
+---------+
| w1 + 12 |
+---------+

After input 2 (push)
+-------------+
| w2 + 4      |
+-------------+
| w1 + 12 * 26|
+-------------+

After input 3 (push)
+------------------+
| w3 + 8           |
+------------------+
| w2 + 4  * 26     |
+------------------+
| w1 + 12 * 26 * 26|
+------------------+

After input 4 (pop)
+-------------+
| w2 + 4      |
+-------------+
| w1 + 12 * 26|
+-------------+

After input 5 (pop)
+---------+
| w1 + 12 |
+---------+

After input 6 (pop)
z should be 0


The stack means that each input has impact on one other input. In our example this means
input 1 is tied to input 6
input 2 is tied to input 5
input 3 is tied to input 4

So w' + var2' + var1" == w":
w1 + 12 + -14 == w6
w2 +  4 + -11 == w5
w3 +  8 +  -3 == w4

This means that
w1 can be 9 at most, since 9 + 12 - 14 = 7 (where 1 <= 7 <= 9). This means that when w1 is 9, w6 should be 7 to satisfy x == w
w2 can be 9 at most, since 9 + 4 - 11 = 2. This means that when w2 is 9, w5 should be 2
w3 can be 4 at most since, 4 + 8 - 3 = 9. This means that when w3 is 4, w6 should be 9

So highest input for this example would be: 994927

And just because we can an ALU was programmed...
*/