using AdventOfCode2022;
using AdventOfCode2022.Days;

var daysMap = new Dictionary<ushort, AoC>
{
    { 1, new Day1() },
    { 2, new Day2() },
    { 3, new Day3() },
    { 4, new Day4() },
    { 5, new Day5() },
    { 6, new Day6() },
    { 7, new Day7() },
    { 8, new Day8() },
    { 9, new Day9() },
    { 10, new Day10() },
    { 11, new Day11() },
    { 12, new Day12() },
    { 13, new Day13() },
    { 14, new Day14() },
};

while (true)
{
    Console.WriteLine("Which day would you like to run? (Choose a number 1-25 or type 'q' to quit)");
    var input = Console.ReadLine();

    if (input == "q") break;

    if (ushort.TryParse(input, out var day))
    {
        if (day > 25)
        {
            Console.WriteLine("\nThat day is bigger than 25");
            return;
        }

        // TODO: Remove the GetValueOrDefault once you have all the solutions
        daysMap.GetValueOrDefault(day)?.Run();
    }
    else
    {
        Console.WriteLine("\nThat is not a valid input");
    }
}