namespace ProcGenFun.Mazes;

using System.Collections.Immutable;
using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class BinaryTree
{
    public static IDistribution<Maze> MazeDistribution(Grid grid)
    {
        return StateDistribution(grid).Select(s => s.Maze);
    }

    public static IDistribution<IEnumerable<Maze>> MazesDistribution(Grid grid)
    {
        return StateDistribution(grid).Select(s => s.Mazes.Append(s.Maze));
    }

    private static IDistribution<State> StateDistribution(Grid grid)
    {
        var initialState = new State([], Maze.WithAllWalls(grid));

        IDistribution<State> stateDist = Singleton.New(initialState);

        foreach (var cell in grid.Cells)
        {
            stateDist = NextStepDistribution(stateDist, grid, cell);
        }

        return stateDist;
    }

    private static IDistribution<State> NextStepDistribution(
        IDistribution<State> stateDist, Grid grid, Cell cell)
    {
        var validDirections = GetValidDirections(cell, grid);

        return UniformDistribution.TryCreate(validDirections, out var directionDist)
            ? (from state in stateDist
               from direction in directionDist
               select new State(Mazes: state.Mazes.Add(state.Maze), Maze: state.Maze.RemoveWall(cell, direction)))
            : stateDist;
    }

    private static IEnumerable<Direction> GetValidDirections(Cell cell, Grid grid) =>
        from dir in new[] { Direction.North, Direction.East }
        where grid.AdjacentCellOrNull(cell, dir) != null
        select dir;

    private record State(ImmutableList<Maze> Mazes, Maze Maze);
}
