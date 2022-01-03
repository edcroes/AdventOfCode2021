namespace AoC2021.Day16;

public interface IPacket
{
    short Version { get; }
    int PacketLength { get; }
    long Value { get; }
}

public record LiteralPacket(short Version, long Literal, int PacketLength) : IPacket
{
    public long Value => Literal;
}

public abstract class OperatorPacket : IPacket
{
    public static readonly short[] LengthTypeSizes = { 15, 11 };
    public int BlablaLength { get; set; }
    public short Version { get; init; }
    public short LengthTypeId { get; init; }
    public short LengthTypeSize => LengthTypeSizes[LengthTypeId];
    public List<IPacket> SubPackets { get; } = new();

    public int PacketLength => 3 + 3 + 1 + LengthTypeSize + SubPackets.Sum(p => p.PacketLength);

    public abstract long Value { get; }
}

public class SumPacket : OperatorPacket
{
    public override long Value => SubPackets.Sum(p => p.Value);
}

public class ProductPacket : OperatorPacket
{
    public override long Value => SubPackets.Aggregate(1L, (value, packet) => value * packet.Value);
}

public class MinimumPacket : OperatorPacket
{
    public override long Value => SubPackets.Min(p => p.Value);
}

public class MaximumPacket : OperatorPacket
{
    public override long Value => SubPackets.Max(p => p.Value);
}

public class GreaterThanPacket : OperatorPacket
{
    public override long Value => SubPackets.First().Value > SubPackets.Last().Value ? 1 : 0;
}

public class LessThanPacket : OperatorPacket
{
    public override long Value => SubPackets.First().Value < SubPackets.Last().Value ? 1 : 0;
}

public class EqualToPacket : OperatorPacket
{
    public override long Value => SubPackets.First().Value == SubPackets.Last().Value ? 1 : 0;
}