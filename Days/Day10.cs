using AdventOfCode2022.Attributes;

namespace AdventOfCode2022.Days;

[ProblemName("Cathode-Ray Tube")]
public class Day10 : AoC
{
    private static readonly Dictionary<string, (int, bool)> MapSignalToAction = new()
    {
        { "addx", (2, true) },
        { "noop", (1, false) }
    };

    protected override string Part1(string input)
    {
        var clocks = 0;
        var registerX = 1;
        var result = 0;
        
        foreach (var signals in input.Split("\r\n").Select(l => l.Split(" ")))
        {
            var (cycles, hasValue) = MapSignalToAction[signals[0]];

            for (var i = 0; i < cycles; i++)
            {
                clocks++;
                if (clocks % 20 == 0 && clocks % 40 != 0)
                    result += clocks * registerX;
            }
            registerX += hasValue ? int.Parse(signals[1]) : 0;
        }
        
        return result.ToString();
    }

    protected override string Part2(string input)
    {
        var clocks = 0;
        var spritePosition = 1;
        var crt = "\n\n";
        
        foreach (var signals in input.Split("\r\n").Select(l => l.Split(" ")))
        {
            var (cycles, hasValue) = MapSignalToAction[signals[0]];

            for (var i = 0; i < cycles; i++)
            {
                clocks++;
                crt += IsPixelWithinSprite(spritePosition, crt, clocks) ? "#" : ".";
                if (clocks % 40 == 0)
                    crt += "\n";
            }
            spritePosition += hasValue ? int.Parse(signals[1]) : 0;
        }
        
        return crt;
    }

    private static bool IsPixelWithinSprite(int spritePosition, string crt, int clocks)
    {
        var crtSize = crt.Count(s => s != '\n') + 1 - clocks / 40 * 40;
        return crtSize >= spritePosition && crtSize <= spritePosition + 2;
    }
}