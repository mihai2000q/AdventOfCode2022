using AdventOfCode2022.Attributes;

namespace AdventOfCode2022.Days;

[ProblemName("Supply Stack")]
public class Day5 : AoC
{
    protected override string Part1(string input)
    {
        return MoveCrates(input, CrateMover9000);
    }

    protected override string Part2(string input)
    {
        return MoveCrates(input, CrateMover9001);
    }

    private static string MoveCrates(string input, Action<Stack<char>[], int, int, int> moveCrates)
    {
        var parts = input.Split("\r\n\r\n");
        var stacksDef = parts[0].Split("\r\n");
        var moves = parts[1];

        var stacks = GetStacks(stacksDef);

        foreach (var line in moves.Split("\r\n"))
        {
            var move = line.Split(" ");
            var number = int.Parse(move[1]);
            var src = int.Parse(move[3]) - 1;
            var dest = int.Parse(move[5]) - 1;

            moveCrates(stacks, number, dest, src);
        }

        return string.Join("", stacks.Select(s => s.Peek()));
    }

    private static Stack<char>[] GetStacks(string[] stacksDef)
    {
        var stacks = stacksDef
            .Last()
            .Chunk(4)
            .Select(_ => new Stack<char>())
            .ToArray();

        foreach (var line in stacksDef.Reverse().Skip(1))
        {
            foreach (var (stack, str) in stacks.Zip(line.Chunk(4)))
            {
                if (!char.IsWhiteSpace(str[1]))
                {
                    stack.Push(str[1]);
                }
            }
        }

        return stacks;
    }

    private static void CrateMover9000(Stack<char>[] stacks, int number, int dest, int src)
    {
        for (var i = 0; i < number; i++)
        {
            stacks[dest].Push(stacks[src].Pop());
        }
    }

    private static void CrateMover9001(Stack<char>[] stacks, int number, int dest, int src)
    {
        var helper = new Stack<char>();
        for (var i = 0; i < number; i++)
        {
            helper.Push(stacks[src].Pop());
        }

        var count = helper.Count;
        for (var i = 0; i < count; i++)
        {
            stacks[dest].Push(helper.Pop());
        }
    }
}