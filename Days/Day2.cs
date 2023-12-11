using AdventOfCode2022.Attributes;

namespace AdventOfCode2022.Days;

[ProblemName("Rock Paper Scissors")]
public class Day2 : AoC
{
    private enum Sign
    {
        Rock = 1,
        Paper = 2,
        Scissor = 3,
    }

    protected override string Part1(string input)
    {
        return input
            .Split("\r\n")
            .Select(line => Score(Elf(line[0], new[] { 'A', 'B', 'C' }),
                Elf(line[2], new[] { 'X', 'Y', 'Z' })))
            .Sum()
            .ToString();
    }

    protected override string Part2(string input)
    {
        return input
            .Split("\r\n")
            .Select(line => Score(Elf(line[0], new[] { 'A', 'B', 'C' }), 
                WhichSign(Elf(line[0], new[] { 'A', 'B', 'C' }), line[2])))
            .Sum()
            .ToString();
    }

    private static Sign Elf(char line, IList<char> signs) =>
        line == signs[0] ? Sign.Rock :
        line == signs[1] ? Sign.Paper :
        line == signs[2] ? Sign.Scissor :
        throw new ArgumentException(line.ToString());

    private static Sign WhichSign(Sign elf, char outcome) =>
        outcome == 'X' ? Next(Next(elf)) : // loss
        outcome == 'Y' ? elf : // draw
        outcome == 'Z' ? Next(elf) : // win
        throw new ArgumentException("");
    
    private static Sign Next(Sign sign) =>
        sign == Sign.Rock ? Sign.Paper :
        sign == Sign.Paper ? Sign.Scissor :
        sign == Sign.Scissor ? Sign.Rock :
        throw new ArgumentException("");
    
    private static int Score(Sign elf, Sign human) =>
        human == Next(elf) ? (int)human + 6 : // win
        human == elf ? (int)human + 3 : // draw
        (int)human; // loss
}