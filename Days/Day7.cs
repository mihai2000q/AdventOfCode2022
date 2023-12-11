using AdventOfCode2022.Attributes;

namespace AdventOfCode2022.Days;

[ProblemName("No Space Left On Device")]
public class Day7 : AoC
{
    protected override string Part1(string input)
    {
        return GetDirectorySizes(input).Values.Where(a => a < 100_000).Sum().ToString();
    }

    protected override string Part2(string input)
    {
        var directorySizes = GetDirectorySizes(input);
        var spaceUsed = directorySizes.Values.Max();
        var sizeToDelete = 30_000_000 - (70_000_000 - spaceUsed);
        return directorySizes.Values.Order().First(x => x >= sizeToDelete).ToString();
    }

    private static Dictionary<string, int> GetDirectorySizes(string input)
    {
        var directorySizes = new Dictionary<string, int>();
        var path = new Stack<string>();
        foreach (var line in input.Split("\r\n"))
        {
            var symbols = line.Split(" ");

            if (symbols[1] == "cd")
            {
                if (symbols[2] == "..")
                {
                    path.Pop();
                }
                else
                {
                    path.Push(string.Join("", path) + symbols[2]);
                }
            }

            if (!int.TryParse(symbols[0], out var size)) continue;
            foreach (var dir in path)
            {
                directorySizes[dir] = directorySizes.GetValueOrDefault(dir) + size;
            }
        }

        return directorySizes;
    }
}