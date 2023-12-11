using AdventOfCode2022.Attributes;

namespace AdventOfCode2022.Days;

[ProblemName("Rucksack Reorganization")]
public class Day3 : AoC
{
    protected override string Part1(string input)
    {
        return input
            .Split("\r\n")
            .Select(s => new { FirstCompartment = s[..(s.Length / 2)], Second = s[(s.Length / 2)..] })
            .Select(r => r.FirstCompartment
                .Where(r.Second.Contains)
                .Distinct()
                .First())
            .Select(Priority)
            .Sum()
            .ToString();
    }

    protected override string Part2(string input)
    {
        return input
            .Split("\r\n")
            .Chunk(3)
            .Select(rucksack => rucksack
                .First()
                .First(x => rucksack.All(c => c.Contains(x)))
            )
            .Select(Priority)
            .Sum()
            .ToString();
    }
    
    private static string Part1Alternative(string input)
    {
        return input
            .Split("\r\n")
            .Select(line => line.Chunk(line.Length / 2))
            .Select(rucksack => rucksack
                .First()
                .First(x => rucksack.All(c => c.Contains(x)))
            )
            .Select(Priority)
            .Sum()
            .ToString();
    }

    private static int Priority(char x) =>
        char.IsUpper(x) ? x - 'A' + 27 :
        char.IsLower(x) ? x - 'a' + 1 :
        throw new ArgumentNullException(x.ToString());

}