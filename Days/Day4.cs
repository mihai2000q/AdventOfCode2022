using AdventOfCode2022.Attributes;

namespace AdventOfCode2022.Days;

[ProblemName("Camp Cleanup")]
public class Day4 : AoC
{
    protected override string Part1(string input)
    {
        return input
            .Split("\r\n")
            .Select(line => new { Pair1 = line.Split(",")[0], Pair2 = line.Split(",")[1] })
            .Select(p => new
                {
                    Range1 = int.Parse(p.Pair1.Split("-")[0])..int.Parse(p.Pair1.Split("-")[1]),
                    Range2 = int.Parse(p.Pair2.Split("-")[0])..int.Parse(p.Pair2.Split("-")[1]),
                }
            )
            .Select(r => 
                RangeContains(r.Range1, r.Range2) ||
                RangeContains(r.Range2, r.Range1) ? 
                    1 : 
                    0)
            .Sum()
            .ToString();
    }

    protected override string Part2(string input)
    {
        return input
            .Split("\r\n")
            .Select(line => new { Pair1 = line.Split(",")[0], Pair2 = line.Split(",")[1] })
            .Select(p => new
                {
                    Range1 = int.Parse(p.Pair1.Split("-")[0])..int.Parse(p.Pair1.Split("-")[1]),
                    Range2 = int.Parse(p.Pair2.Split("-")[0])..int.Parse(p.Pair2.Split("-")[1]),
                }
            ).Select(r => 
                RangeOverlaps(r.Range1, r.Range2) ||
                RangeOverlaps(r.Range2, r.Range1) ? 
                    1 : 
                    0)
            .Sum()
            .ToString();
    }

    private static bool RangeContains(Range r1, Range r2) =>
        r1.Start.Value <= r2.Start.Value && r1.End.Value >= r2.End.Value;
    
    private static bool RangeOverlaps(Range r1, Range r2) =>
        r1.Start.Value <= r2.End.Value && r1.End.Value >= r2.Start.Value;

    private static string Part1Alternative(string input)
    {
        return input
            .Split("\r\n")
            .Select(line => new { Pair1 = line.Split(",")[0], Pair2 = line.Split(",")[1] })
            .Select(p => new
                {
                    List1 = Enumerable.Range(int.Parse(p.Pair1.Split("-")[0].ToString()),
                        int.Parse(p.Pair1.Split("-")[1].ToString()) 
                        - int.Parse(p.Pair1.Split("-")[0].ToString()) + 1),
                    List2 = Enumerable.Range(int.Parse(p.Pair2.Split("-")[0].ToString()),
                        int.Parse(p.Pair2.Split("-")[1].ToString()) 
                        - int.Parse(p.Pair2.Split("-")[0].ToString()) + 1)
                }
            ).Select(l => 
                l.List2.All(l.List1.Contains) ||
                l.List1.All(l.List2.Contains) ? 
                    1 : 
                    0)
            .Sum()
            .ToString();
    }
    
    private static string Part2Alternative(string input)
    {
        return input
            .Split("\r\n")
            .Select(line => new { Pair1 = line.Split(",")[0], Pair2 = line.Split(",")[1] })
            .Select(p => new
                {
                    List1 = Enumerable.Range(int.Parse(p.Pair1.Split("-")[0].ToString()),
                        int.Parse(p.Pair1.Split("-")[1].ToString())
                        - int.Parse(p.Pair1.Split("-")[0].ToString()) + 1),
                    List2 = Enumerable.Range(int.Parse(p.Pair2.Split("-")[0].ToString()),
                        int.Parse(p.Pair2.Split("-")[1].ToString())
                        - int.Parse(p.Pair2.Split("-")[0].ToString()) + 1)
                }
            ).Select(l =>
                l.List2.Any(l.List1.Contains) ||
                l.List1.Any(l.List2.Contains)
                    ? 1
                    : 0)
            .Sum()
            .ToString();
    }
}