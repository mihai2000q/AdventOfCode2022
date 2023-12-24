using System.Text;
using AdventOfCode2022.Attributes;

namespace AdventOfCode2022.Days;

[ProblemName("Distress Signal")]
public class Day13 : AoC
{

    private abstract record Packet : IComparable<Packet>
    {
        public abstract int CompareTo(Packet? other);
    }

    private record IntPacket(int Amount) : Packet
    {
        public override int CompareTo(Packet? other)
        {
            return other switch
            {
                IntPacket packet => Amount.CompareTo(packet.Amount),
                ListPacket packet => new ListPacket(new List<Packet>() { this }).CompareTo(packet),
                _ => -1
            };
        }

        public override string ToString()
        {
            return Amount.ToString();
        }
    }

    private record ListPacket(List<Packet> SubPackets) : Packet
    {
        public override int CompareTo(Packet? other)
        {
            return other switch
            {
                IntPacket packet => CompareTo(new ListPacket(new List<Packet>() { packet })),
                ListPacket packet => Compare2Lists(packet),
                _ => -1
            };
        }

        private int Compare2Lists(ListPacket packet)
        {
            for (var i = 0; i < SubPackets.Count; i++)
            {
                if (i == packet.SubPackets.Count)
                {
                    return 1;
                }

                var res = SubPackets[i].CompareTo(packet.SubPackets[i]);
                if (res != 0)
                {
                    return res;
                }
            }

            if (SubPackets.Count < packet.SubPackets.Count)
            {
                return -1;
            }

            return 0;
        }

        public override string ToString()
        {
            return $"[ {string.Join(", ", SubPackets.Select(p => p.ToString()))} ]";
        }
    }

    protected override string Part1(string input)
    {
        var pairsOfPackets = ParsePackets(input);

        var indices = new List<int>();

        for (int i = 0; i < pairsOfPackets.Count; i++)
        {
            var packet1 = pairsOfPackets[i].First();
            var packet2 = pairsOfPackets[i].Last();

            if (packet1.CompareTo(packet2) == -1)
            {
                indices.Add(i + 1);
            }
        }

        return indices.Sum().ToString();
    }

    protected override string Part2(string input)
    {
        var dividerPacket1 = ToPacket("[[2]]");
        var dividerPacket2 = ToPacket("[[6]]");

        var pairsOfPackets = ParsePackets(input)
            .SelectMany(x => x)
            .ToList();
        pairsOfPackets.Add(dividerPacket1);
        pairsOfPackets.Add(dividerPacket2);
        pairsOfPackets = pairsOfPackets
            .Order()
            .ToList();

        return ((pairsOfPackets.IndexOf(dividerPacket1) + 1) * (pairsOfPackets.IndexOf(dividerPacket2) + 1)).ToString();
    }

    private static IList<IEnumerable<Packet>> ParsePackets(string input)
    {
        return input
            .Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Split("\r\n"))
            .Select(s => s.Select(ToPacket))
            .ToList();
    }

    private static Packet ToPacket(string input)
    {
        var subPackets = new Stack<List<Packet>>();

        var currentElement = new StringBuilder();

        foreach (var character in input)
        {
            if (character == '[')
            {
                subPackets.Push(new List<Packet>());
            }
            else if (character == ']' || character == ',')
            {
                if (!string.IsNullOrEmpty(currentElement.ToString()))
                {
                    var number = int.Parse(currentElement.ToString());
                    var packet = new IntPacket(number);
                    subPackets.Peek().Add(packet);
                    currentElement.Clear();
                }

                if (character == ']' && subPackets.Count != 1)
                {
                    var packet = new ListPacket(subPackets.Pop());
                    subPackets.Peek().Add(packet);
                }
            }
            else
            {
                currentElement.Append(character);
            }
        }

        return new ListPacket(subPackets.Pop());
    }
}