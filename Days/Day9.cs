using AdventOfCode2022.Attributes;

namespace AdventOfCode2022.Days;

[ProblemName("Rope Bridge")]
public class Day9 : AoC
{
    private class Knot
    {
        public string Symbol { get; init; } = null!;
        public int Row { get; set; }
        public int Col { get; set; }
        public HashSet<(int, int)> History { get; } = new();

        public override string ToString()
        {
            return $"{Symbol} is at Row: {Row} and Col: {Col}";
        }
    }

    private enum Position
    {
        UpLeft,
        Up,
        UpRight,
        Left,
        Overlap,
        Right,
        DownLeft,
        Down,
        DownRight
    }
    
    private static readonly Dictionary<string, Action<Knot>> MapHeadDirections = new()
    {
        { "U", r => { r.Row -= 1; } },
        { "D", r => { r.Row += 1; } },
        { "L", r => { r.Col -= 1; } },
        { "R", r => { r.Col += 1; } },
    };
    
    private static readonly Dictionary<Position, Action<Knot>> MapTailDirections = new()
    {   
        { Position.UpLeft, r => { r.Row -= 1; r.Col -= 1; } },
        { Position.Up, r => { r.Row -= 1; } },
        { Position.UpRight, r => { r.Row -= 1; r.Col += 1; } },
        { Position.Left, r => { r.Col -= 1; } },
        { Position.Overlap, _ => {  } },
        { Position.Right, r => { r.Col += 1; } },
        { Position.DownLeft, r => { r.Row += 1; r.Col -= 1; } },
        { Position.Down, r => { r.Row += 1; } },
        { Position.DownRight, r => { r.Row += 1; r.Col += 1; } },
    };

    protected override string Part1(string input)
    {
        return KnotTheRope(input, 1);
    }

    protected override string Part2(string input)
    {
        return KnotTheRope(input, 9);
    }

    private static string KnotTheRope(string input, int numberOfTails)
    {
        var directionAndTimes = input
            .Split("\r\n")
            .Select(s => s
                .Split(" "))
            .Select(x => new { Direction = x.First(), Times = int.Parse(x.Last()) })
            .ToList();
        
        var head = new Knot { Symbol = "H", Row = 5, Col = 1 };
        var tails = Enumerable.Range(1, numberOfTails)
            .Select(n => new Knot { Symbol = n.ToString(), Row = 5, Col = 1 })
            .ToList();
        
        foreach (var dt in directionAndTimes)
        {
            // Console.WriteLine($"=== {dt.Direction} {dt.Times} ===");
            foreach (var _ in Enumerable.Range(0, dt.Times))
            {
                MapHeadDirections[dt.Direction].Invoke(head);
                AdvanceAndRemember(head, tails[0]);
                for (var i = 0; i < tails.Count - 1; i++)
                {
                    AdvanceAndRemember(tails[i], tails[i+1]);
                }
                // PrintGrid(head, tails);
            }
        }
        
        return tails.Last().History.Count.ToString();
    }

    private static void AdvanceAndRemember(Knot head, Knot tail)
    {
        if (!AreTouching(head, tail))
            MapTailDirections[WhereIsHead(head, tail)].Invoke(tail);

        tail.History.Add((tail.Row, tail.Col));
    }
    
    private static bool AreTouching(Knot head, Knot tail)
    {
        return (Math.Abs(head.Row - tail.Row) == 1 || head.Row - tail.Row == 0) && 
               (Math.Abs(head.Col - tail.Col) == 1 || head.Col - tail.Col == 0);
    }

    private static Position WhereIsHead(Knot head, Knot tail)
    {
        if (head.Row < tail.Row && head.Col < tail.Col)
            return Position.UpLeft;
        if (head.Row < tail.Row && head.Col == tail.Col)
            return Position.Up;
        if (head.Row < tail.Row && head.Col > tail.Col)
            return Position.UpRight;
        
        if (head.Row == tail.Row && head.Col < tail.Col)
            return Position.Left;
        if (head.Row == tail.Row && head.Col > tail.Col)
            return Position.Right;
        
        if (head.Row > tail.Row && head.Col < tail.Col)
            return Position.DownLeft;
        if (head.Row > tail.Row && head.Col == tail.Col)
            return Position.Down;
        if (head.Row > tail.Row && head.Col > tail.Col)
            return Position.DownRight;
        
        return Position.Overlap;
    }
    
    // only works for Part1
    private static void PrintGrid(Knot head, List<Knot> tails)
    {
        for (var i = 1; i <= 5; i++)
        {
            for (var j = 1; j <= 6; j++)
            {
                if (head.Row == i && head.Col == j)
                {
                    Console.Write(head.Symbol);
                    continue;
                }
                
                foreach (var tail in tails)
                {
                    if (tail.Row == i && tail.Col == j) 
                        Console.Write(tail.Symbol);
                    else if (i == 5 && j == 1)
                        Console.Write("s");
                    else
                        Console.Write(".");
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

}