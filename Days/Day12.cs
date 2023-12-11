using AdventOfCode2022.Attributes;

namespace AdventOfCode2022.Days;

[ProblemName("Hill Climbing Algorithm")]
public class Day12 : AoC
{
    private record Elevation(char Symbol, int Row, int Col);

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private static readonly Dictionary<Direction, (int, int)> MapDirectionToElevation = new()
    {
        { Direction.Up, (-1, 0) },
        { Direction.Down, (1, 0) },
        { Direction.Left, (0, -1) },
        { Direction.Right, (0, 1) },
    };

    protected override string Part1(string input)
    {
        var elevations = ParseElevations(input);
        var start = elevations.Single(e => e.Symbol == 'S');
        var goal = elevations.Single(e => e.Symbol == 'E');

        return (GetDijkstraPath(elevations, start, goal).Count - 1).ToString();
    }

    protected override string Part2(string input)
    {
        var elevations = ParseElevations(input);
        var starts = elevations
            .Where(e => e.Symbol == 'a')
            .ToList();
        starts.Add(elevations.Single(e => e.Symbol == 'S'));
        var goal = elevations.Single(e => e.Symbol == 'E');

        var paths = starts
            .Select(start => GetDijkstraPath(elevations, start, goal))
            .Where(x => x.Count > 0)
            .ToList();

        var minimum = paths.MinBy(x => x.Count);

        return (minimum!.Count - 1).ToString();
    }

    private static IList<Elevation> ParseElevations(string input)
    {
        var heightMap = input.Split("\r\n");
        var elevations = new List<Elevation>();

        for (var i = 0; i < heightMap.Length; i++)
        {
            for (var j = 0; j < heightMap[i].Length; j++)
            {
                elevations.Add(new Elevation(heightMap[i][j], i, j));
            }
        }

        return elevations;
    }

    // ReSharper disable once UnusedMember.Local
    private static List<Elevation> GetBfsPath(IList<Elevation> elevations, Elevation start, Elevation goal)
    {
        var predecessors = new Dictionary<Elevation, Elevation>();

        var queue = new Queue<Elevation>();
        queue.Enqueue(start);

        while (queue.Count != 0)
        {
            var currentElevation = queue.Dequeue();

            if (currentElevation.Symbol == goal.Symbol)
                return CalculatePath(predecessors, start, goal);

            foreach (var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
            {
                var neighbour = GetElevationFromDirection(elevations, direction, currentElevation);

                if (neighbour is null)
                    continue;

                if (predecessors.ContainsKey(neighbour) || !IsLegalPosition(currentElevation, neighbour))
                    continue;

                queue.Enqueue(neighbour);
                predecessors[neighbour] = currentElevation;
            }
        }

        return new List<Elevation>();
    }

    private static List<Elevation> GetDijkstraPath(IList<Elevation> elevations, Elevation start, Elevation goal)
    {
        var predecessors = new Dictionary<Elevation, Elevation>();

        var priorityQueue = new PriorityQueue<Elevation, int>();
        priorityQueue.Enqueue(start, 0);

        var gValues = new Dictionary<Elevation, int>()
        {
            { start, 0 }
        };

        while (priorityQueue.Count != 0)
        {
            var currentElevation = priorityQueue.Dequeue();

            if (currentElevation.Symbol == goal.Symbol)
                return CalculatePath(predecessors, start, goal);

            foreach (var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
            {
                var neighbour = GetElevationFromDirection(elevations, direction, currentElevation);

                if (neighbour is null) continue;

                if (!IsLegalPosition(currentElevation, neighbour)) continue;
                
                if (gValues.ContainsKey(neighbour)) continue;

                var newCost = gValues[currentElevation] + 1;
                gValues[neighbour] = newCost;
                priorityQueue.Enqueue(neighbour, newCost);
                predecessors[neighbour] = currentElevation;
            }
        }

        return new List<Elevation>();
    }

    private static Elevation? GetElevationFromDirection(
        IEnumerable<Elevation> elevations,
        Direction direction,
        Elevation currentElevation)
    {
        var coords = MapDirectionToElevation[direction];

        return elevations.SingleOrDefault(e => e.Row == currentElevation.Row + coords.Item1 &&
                                               e.Col == currentElevation.Col + coords.Item2);
    }

    private static bool IsLegalPosition(Elevation current, Elevation next)
    {
        if (current.Symbol == 'S' && next.Symbol == 'a')
            return true;

        if (next.Symbol == 'E')
            return current.Symbol == 'z';

        var value = next.Symbol - current.Symbol;
        return value <= 1;
    }

    private static List<Elevation> CalculatePath(
        IReadOnlyDictionary<Elevation, Elevation> predecessors,
        Elevation start,
        Elevation goal
    )
    {
        var current = goal;
        var path = new List<Elevation>();
        while (current != start)
        {
            path.Add(current);
            current = predecessors[current];
        }

        path.Add(start);
        path.Reverse();

        return path;
    }
}