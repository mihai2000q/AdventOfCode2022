using AdventOfCode2022.Attributes;

namespace AdventOfCode2022.Days;

[ProblemName("Tuning Trouble")]
public class Day6 : AoC
{
    protected override string Part1(string input)
    {
        return StartOfMarker(input, 4);
    }

    protected override string Part2(string input)
    {
        return StartOfMarker(input, 14);
    }

    private static string StartOfMarker(string input, int size) =>
    Enumerable.Range(size, input.Length)
            .First(s => input.Substring(s - size, size).ToHashSet().Count == size)
            .ToString();
}