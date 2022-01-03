namespace AoC2021.Day16;

public class PacketParser
{
    private const short VersionHeaderSize = 3;
    private const short TypeHeaderSize = 3;
    private const short LiteralGroupSize = 5;
    private const short MinimunPacketSize = VersionHeaderSize + TypeHeaderSize + LiteralGroupSize;

    private static readonly Dictionary<char, string> _hexToBinary = new()
    {
        { '0', "0000" },
        { '1', "0001" },
        { '2', "0010" },
        { '3', "0011" },
        { '4', "0100" },
        { '5', "0101" },
        { '6', "0110" },
        { '7', "0111" },
        { '8', "1000" },
        { '9', "1001" },
        { 'A', "1010" },
        { 'B', "1011" },
        { 'C', "1100" },
        { 'D', "1101" },
        { 'E', "1110" },
        { 'F', "1111" }
    };

    public List<IPacket> Parse(string packets)
    {
        var binary = string.Join("", packets.ToCharArray().Select(c => _hexToBinary[c]));
        return ParsePackets(binary);
    }

    private List<IPacket> ParsePackets(string binary, int maxNumberOfPackets = int.MaxValue)
    {
        List<IPacket> packets = new();

        while (binary.Length >= MinimunPacketSize && packets.Count < maxNumberOfPackets)
        {
            var version = Convert.ToInt16(binary[..VersionHeaderSize], 2);
            short type = Convert.ToInt16(binary[VersionHeaderSize..(VersionHeaderSize + TypeHeaderSize)], 2);

            var packet = type switch
            {
                4 => ParseLiteralPacket(version, binary),
                _ => ParseOperatorPacket(version, type, binary)
            };
            packets.Add(packet);

            binary = binary[packet.PacketLength..];
        }

        return packets;
    }

    private IPacket ParseLiteralPacket(short version, string binary)
    {
        var valueString = string.Empty;
        var packetLength = VersionHeaderSize + TypeHeaderSize;
        var isLastGroupAdded = false;

        while (!isLastGroupAdded)
        {
            var current = binary[packetLength..(packetLength + LiteralGroupSize)];
            valueString += current[1..];
            packetLength += LiteralGroupSize;

            isLastGroupAdded = current[0] == '0';
        }

        return new LiteralPacket(version, Convert.ToInt64(valueString, 2), packetLength);
    }

    private IPacket ParseOperatorPacket(short version, short typeId, string binary)
    {
        var lengthTypeId = Convert.ToByte(binary[VersionHeaderSize + TypeHeaderSize].ToString(), 2);
        var packet = CreateOperatorPacket(version, lengthTypeId, typeId);

        var lengthTypeNumberStart = VersionHeaderSize + TypeHeaderSize + 1;
        var lengthTypeNumberEnd = lengthTypeNumberStart + OperatorPacket.LengthTypeSizes[lengthTypeId];
        var lengthTypeNumber = Convert.ToInt16(binary[lengthTypeNumberStart..lengthTypeNumberEnd], 2);

        var subPackets = lengthTypeId switch
        {
            1 => ParsePackets(binary[lengthTypeNumberEnd..], lengthTypeNumber),
            _ => ParsePackets(binary[lengthTypeNumberEnd..(lengthTypeNumberEnd + lengthTypeNumber)])
            
        };
        packet.SubPackets.AddRange(subPackets);

        return packet;
    }

    private static OperatorPacket CreateOperatorPacket(short version, short lengthTypeId, short typeId) =>
        typeId switch
        {
            0 => new SumPacket { Version = version, LengthTypeId = lengthTypeId },
            1 => new ProductPacket { Version = version, LengthTypeId = lengthTypeId },
            2 => new MinimumPacket { Version = version, LengthTypeId = lengthTypeId },
            3 => new MaximumPacket { Version = version, LengthTypeId = lengthTypeId },
            5 => new GreaterThanPacket { Version = version, LengthTypeId = lengthTypeId },
            6 => new LessThanPacket { Version = version, LengthTypeId = lengthTypeId },
            7 => new EqualToPacket { Version = version, LengthTypeId = lengthTypeId },
            _ => throw new InvalidOperationException($"TypeId {typeId} is unknown")
        };
}