using AdventOfCode2022.Attributes;

namespace AdventOfCode2022.Days;

[ProblemName("Treetop Tree House")]
public class Day8 : AoC
{
    private record Tree(int Height, int Row, int Col);

    private enum Direction
    {
        Up,
        Left,
        Right,
        Down
    }

    private static readonly Dictionary<Direction, Func<Tree, int, int, bool>> DirectionsMap = new()
    {
        { Direction.Up, (t, row, col) => t.Row < row && t.Col == col },
        { Direction.Left, (t, row, col) => t.Row == row && t.Col < col },
        { Direction.Right, (t, row, col) => t.Row == row && t.Col > col },
        { Direction.Down, (t, row, col) => t.Row > row && t.Col == col }
    };

    protected override string Part1(string input)
    {
        var forest = GetForest(input);
        var count = (forest.Count - 1) * 4;

        for (var i = 1; i < forest.Count - 1; i++)
        for (var j = 1; j < forest.Count - 1; j++)
            CountSmallerTrees(forest, i, j, ref count);

        return count.ToString();
    }

    protected override string Part2(string input)
    {
        var scenicScores = new List<int>();
        var forest = GetForest(input);

        for (var i = 1; i < forest.Count - 1; i++)
        for (var j = 1; j < forest.Count - 1; j++)
            CalculateScenicScore(forest, i, j, ref scenicScores);

        return scenicScores.Max().ToString();
    }

    private static List<List<Tree>> GetForest(string input)
    {
        var forest = new List<List<Tree>>();
        var row = 0;
        foreach (var line in input.Split("\r\n"))
        {
            var col = 0;
            var trees = line
                .ToCharArray()
                .Select(c => new Tree(int.Parse(c.ToString()), row, col++))
                .ToList();
            forest.Add(trees);
            row++;
        }

        return forest;
    }

    private static IEnumerable<Tree> GetAllTreesFromDirection(IEnumerable<List<Tree>> forest,
        Predicate<Tree> predicate) =>
        forest.SelectMany(f => f.Where(t => predicate(t)));

    private static int GetMaxHeightFromTrees(IEnumerable<List<Tree>> forest,
        Predicate<Tree> predicate) =>
        GetAllTreesFromDirection(forest, predicate).Max(t => t.Height);


    private static void CountSmallerTrees(IList<List<Tree>> forest, int i, int j, ref int count)
    {
        var currentTreeHeight = forest
            .SelectMany(f => f.Where(t => t.Row == i && t.Col == j))
            .First()
            .Height;

        var maximums = DirectionsMap.Values
            .AsParallel()
            .Select(value =>
                GetMaxHeightFromTrees(forest, t => value(t, i, j)))
            .ToList();

        if (maximums.Any(m => m < currentTreeHeight))
            count++;
    }

    private static void CalculateScenicScore(IList<List<Tree>> forest, int i, int j, ref List<int> scenicScores)
    {
        var currentTreeHeight = forest
            .SelectMany(f => f.Where(t => t.Row == i && t.Col == j))
            .First()
            .Height;

        var scores = DirectionsMap
            .AsParallel()
            .Select(keyValue =>
                GetScenicScore(
                    forest,
                    t => keyValue.Value(t, i, j),
                    keyValue.Key,
                    currentTreeHeight)
            )
            .ToList();

        scenicScores.Add(scores.Aggregate(1, (x, y) => x * y));
    }

    private static int GetScenicScore(IEnumerable<List<Tree>> forest,
        Predicate<Tree> predicate, Direction direction, int height)
    {
        var trees = direction is Direction.Left or Direction.Up
            ? GetAllTreesFromDirection(forest, predicate).Reverse()
            : GetAllTreesFromDirection(forest, predicate);

        var score = 0;
        foreach (var tree in trees)
        {
            score++;
            if (tree.Height >= height)
                break;
        }

        return score;
    }
}