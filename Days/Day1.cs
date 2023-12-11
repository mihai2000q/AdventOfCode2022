using AdventOfCode2022.Attributes;

namespace AdventOfCode2022.Days;

[ProblemName("Calorie Counting")]
public class Day1 : AoC
{
    protected override string Part1(string input)
    {
        return FindMostCalories(input, 1);
    }

    protected override string Part2(string input)
    {
        return FindMostCalories(input, 3);
    }

    private static string FindMostCalories(string input, int elves)
    {
        return input
            .TrimEnd()
            .Split("\r\n\r\n")
            .Select(x => x.Split("\r\n")
                .Select(int.Parse)
                .Sum())
            .OrderDescending()
            .Take(elves)
            .Sum()
            .ToString();
    }
}