using AdventOfCode2022.Attributes;

namespace AdventOfCode2022.Days;

[ProblemName("Distress Signal")]
public class Day13 : AoC
{
    protected override string Part1(string input)
    {
        var pairs = ParsePairs(input);

        var indices = new List<int>();

        for (var i = 0; i < pairs.Count; i++)
        {
            var okay = true;
            var curr = pairs[i];
            var map = curr.Item1.Zip(curr.Item2);
            
            if (curr.Item1.Count > curr.Item2.Count) continue;

            foreach (var (first, second) in map)
            {
                foreach (var (ff, ss) in first.Zip(second))
                {
                    if (int.Parse(ff) > int.Parse(ss))
                    {
                        okay = false;
                    }
                }
            }

            if (okay)
            {
                indices.Add(i + 1);
            }
        }

        return indices.Sum().ToString();
    }

    protected override string Part2(string input)
    {
        return "";
    }
    
    private static IList<(List<string[]>, List<string[]>)> ParsePairs(string input)
    {
        return input
            .Split("\r\n\r\n")
            .Select(p => (p.Split("\r\n")[0], p.Split("\r\n")[1]))
            .Select(p => (RemoveBrackets(p.Item1), RemoveBrackets(p.Item2)))
            .Select(p => (SplitElements(p.Item1), SplitElements(p.Item2)))
            .ToList();
    }

    private static string RemoveBrackets(string str) =>
        str
            .Remove(str.Length - 1, 1)
            .Remove(0, 1);

    private static List<string[]> SplitElements(string str)
    {
        var res = new List<string[]>();
        var curr = "";
        var enabled = true;

        foreach (var c in str)
        {
            switch (c)
            {
                case '[':
                {
                    if (enabled)
                        enabled = false;
                    else
                    {
                        res.Add(SplitAndRemoveWhitespaces(curr));
                        curr = "";
                    }

                    continue;
                }
                case ']':
                    enabled = true;
                    continue;
                case ',' when curr.Length > 0 && enabled:
                    res.Add(SplitAndRemoveWhitespaces(curr));
                    curr = "";
                    continue;
                default:
                    curr += c;
                    break;
            }
        }

        if (curr.Length > 0) res.Add(curr.Split(","));

        return res;
    }

    private static string[] SplitAndRemoveWhitespaces(string numbers) =>
        numbers
            .Split(",")
            .Where(n => !string.IsNullOrEmpty(n))
            .ToArray();
}