using System.Text.RegularExpressions;

namespace AoC2021.Day22;

public class Day22 : IMDay
{
    private record struct Instruction(bool On, Cuboid Cuboid);

    private static readonly Regex _instructionRegex = new(@"^(?<state>on|off) x=(?<xFrom>-?\d+)\.\.(?<xTo>-?\d+),y=(?<yFrom>-?\d+)\.\.(?<yTo>-?\d+),z=(?<zFrom>-?\d+)..(?<zTo>-?\d+)$");

    public string FilePath { private get; init; } = "Day22\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var cuboid50 = new Cuboid(new Point3D(-50, -50, -50), new Point3D(50, 50, 50));
        var instructions = (await GetInstructions())
            .Where(i => cuboid50.HasOverlapWith(i.Cuboid))
            .Select(i => i with { Cuboid = i.Cuboid.Intersect(cuboid50) });

        var cuboidsOn = GetCuboidsThatAreOn(instructions);
        return cuboidsOn.Sum(c => c.CubeCount).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var instructions = await GetInstructions();
        var cuboidsOn = GetCuboidsThatAreOn(instructions);

        return cuboidsOn.Sum(c => c.CubeCount).ToString();
    }

    private static List<Cuboid> GetCuboidsThatAreOn(IEnumerable<Instruction> instructions)
    {
        List<Cuboid> cuboidsOn = new();

        foreach (var instruction in instructions)
        {
            if (instruction.On && cuboidsOn.Any(c => c == instruction.Cuboid || c.Contains(instruction.Cuboid)))
            {
                continue;
            }

            var cubesInCube = cuboidsOn.Where(c => instruction.Cuboid.Contains(c)).ToArray();
            cuboidsOn.RemoveRange(cubesInCube);

            List<Cuboid> explodingResults = new();
            var cubesToExplode = cuboidsOn.Where(c => c.HasOverlapWith(instruction.Cuboid)).ToArray();
            foreach (var cube in cubesToExplode)
            {
                explodingResults.AddRange(cube.Explode(instruction.Cuboid));
            }

            cuboidsOn.RemoveRange(cubesToExplode);
            cuboidsOn.AddRange(explodingResults);

            if (instruction.On)
            {
                cuboidsOn.Add(instruction.Cuboid);
            }
        }

        return cuboidsOn;
    }

    private async Task<IEnumerable<Instruction>> GetInstructions() =>
        (await File.ReadAllLinesAsync(FilePath))
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => ParseInstruction(l));

    private static Instruction ParseInstruction(string instruction)
    {
        var match = _instructionRegex.Match(instruction);
        return new(
            match.Groups["state"].Value == "on",
            new Cuboid(
                new Point3D(match.GetInt("xFrom"), match.GetInt("yFrom"), match.GetInt("zFrom")),
                new Point3D(match.GetInt("xTo"), match.GetInt("yTo"), match.GetInt("zTo"))
            )
        );
    }
}