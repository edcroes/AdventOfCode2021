namespace AoC2021.Day24;

public class Alu
{
    private readonly Dictionary<char, int> _registers = new()
    {
        { 'w', 0 },
        { 'y', 0 },
        { 'x', 0 },
        { 'z', 0 }
    };
    private readonly Dictionary<string, Action<char, string>> _instructions;
    private Func<int> _getInput = () => 0;

    public IReadOnlyDictionary<char, int> Registers => _registers;

    public Alu()
    {
         _instructions = new()
         {
             { "inp", (l, r) => _registers[l] = _getInput() },
             { "add", (l, r) => _registers[l] += GetValue(r) },
             { "mul", (l, r) => _registers[l] *= GetValue(r) },
             { "div", (l, r) => _registers[l] /= GetValue(r) },
             { "mod", (l, r) => _registers[l] %= GetValue(r) },
             { "eql", (l, r) => _registers[l] = _registers[l] == GetValue(r) ? 1 : 0 }
         };

        Reset();
    }

    public void Execute(IEnumerable<AluInstruction> instructions, Func<int> getInput)
    {
        _getInput = getInput;
        foreach (var instruction in instructions)
        {
            ExecuteInstruction(instruction);
        }
    }

    public void Reset()
    {
        foreach (var register in _registers.Keys)
        {
            _registers[register] = 0;
        }
    }

    private void ExecuteInstruction(AluInstruction instruction) =>
        _instructions[instruction.Instruction](instruction.Left, instruction.Right);

    private int GetValue(string input) =>
        (input.Length == 1 && input[0] >= 'a' && input[0] <= 'z')
            ? _registers[input[0]]
            : int.Parse(input);
}

public record struct AluInstruction(string Instruction, char Left, string Right);