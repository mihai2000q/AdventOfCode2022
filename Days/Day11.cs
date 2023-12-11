using AdventOfCode2022.Attributes;

namespace AdventOfCode2022.Days;

[ProblemName("Monkey In The Middle")]
public class Day11 : AoC
{
    private static readonly Dictionary<string, Func<long, long, long>> MapOperations = new()
    {
        { "+", (x, y) => x + y },
        { "-", (x, y) => x - y },
        { "*", (x, y) => x * y },
        { "/", (x, y) => x / y },
    };

    protected override string Part1(string input)
    {
        return CalculateMonkeyBusinessLevel(input, 20);
    }

    protected override string Part2(string input)
    {
        return CalculateMonkeyBusinessLevel(input, 10_000);
    }

    private static string CalculateMonkeyBusinessLevel(string input, long rounds)
    {
        var monkeysString = input.Split("\r\n\r\n");
        var monkeys = monkeysString
            .Select(GetItemsByMonkey)
            .ToList();

        var inspections = monkeysString
            .Select(_ => 0L)
            .ToList();
        
        // for part 2
        var superModulo = monkeysString
            .Select(GetTestNumber)
            .Aggregate(1L, (x, y) => x * y);

        for (var i = 0; i < rounds; i++)
        {
            foreach (var monkey in monkeysString)
            {
                var (current, operation, testNumber, trueValue, falseValue) =
                    ParseInputData(monkey);

                foreach (var item in monkeys[current])
                {
                    var itemWorryLevel = InspectItem(operation, item);
                    if (rounds == 20)
                        itemWorryLevel /= 3;
                    else
                        itemWorryLevel %= superModulo;
                    var index = itemWorryLevel % testNumber == 0 ? trueValue : falseValue;
                    monkeys[index].Add(itemWorryLevel);
                    inspections[current]++;
                }

                monkeys[current].Clear();
            }
        }

        return inspections
            .OrderDescending()
            .Take(2)
            .Aggregate((x, y) => x * y)
            .ToString();
    }

    private static List<long> GetItemsByMonkey(string monkey)
    {
        return monkey.Split("\r\n")[1]
            .Split(" ")[4..]
            .Select(s => s.Replace(",", ""))
            .Select(long.Parse)
            .ToList();
    }

    private static int GetTestNumber(string monkey)
    {
        return int.Parse(monkey.Split("\r\n")[3]
            .Split(" ")[5]);
    }

    private static (int, string[], int, int, int) ParseInputData(string monkey)
    {
        var lines = monkey.Split("\r\n");

        var current = int.Parse(lines[0]
            .Split(" ")[1][0].ToString());

        var operation = lines[2]
            .Split(" ")[5..];

        var testNumber = GetTestNumber(monkey);

        var trueValue = int.Parse(lines[4]
            .Split(" ")[9]);

        var falseValue = int.Parse(lines[5]
            .Split(" ")[9]);

        return (current, operation, testNumber, trueValue, falseValue);
    }

    private static long InspectItem(IReadOnlyList<string> operation, long worryLevel)
    {
        var firstOperand = ParseOperand(operation[0], worryLevel);
        var theOperator = operation[1];
        var secondOperand = ParseOperand(operation[2], worryLevel);
        return MapOperations[theOperator].Invoke(firstOperand, secondOperand);
    }

    private static long ParseOperand(string operand, long worryLevel)
    {
        return operand.ToLower().Equals("old") ? worryLevel : long.Parse(operand);
    }
}