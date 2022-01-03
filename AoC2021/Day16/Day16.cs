namespace AoC2021.Day16;

public class Day16 : IMDay
{
    private readonly PacketParser _parser = new();
    public string FilePath { private get; init; } = "Day16\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var packets = await GetPackets();
        var result = GetSumOfAllVersions(packets);

        return result.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var packets = await GetPackets();
        return packets.Single().Value.ToString();
    }

    private int GetSumOfAllVersions(List<IPacket> packets)
    {
        var sum = 0;
        foreach (var packet in packets)
        {
            sum += packet.Version;
            if (packet is OperatorPacket operatorPackage)
            {
                sum += GetSumOfAllVersions(operatorPackage.SubPackets);
            }
        }

        return sum;
    }

    private async Task<List<IPacket>> GetPackets() =>
        _parser.Parse((await File.ReadAllTextAsync(FilePath)).Trim());
}
