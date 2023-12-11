namespace AdventOfCode2022;

public abstract class AoC
{
    private const string Path = "../../../Files";
    private readonly Func<string, string> _fileName = day => $"{Path}/InputDay{day}.txt";

    public void Run()
    {
        var day = ToString()!.Split(".")[^1][3..];
        var filename = _fileName(day);
        var text = "";
        try
        {
            text = File.ReadAllText(filename);
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Input Day file cannot be found");   
        }
        Console.WriteLine();
        Console.WriteLine($"The answer for part 1 is: {Part1(text)}");
        Console.WriteLine($"The answer for part 2 is: {Part2(text)}");
    }

    protected abstract string Part1(string input);
    protected abstract string Part2(string input);
}