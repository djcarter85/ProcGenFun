namespace ProcGenFun.Mazes;

using System.Collections.Immutable;
using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class BinaryTree
{
    public static IDistribution<Maze> MazeDist(Grid grid) =>
        from state in StateDist(grid)
        select state.Current;

    public static IDistribution<BinaryTreeHistory> HistoryDist(Grid grid) =>
        from state in StateDist(grid)
        select new BinaryTreeHistory(
            Initial: state.Initial,
            Steps: state.Steps,
            Final: state.Current);

    private static IDistribution<BinaryTreeState> StateDist(Grid grid)
    {
        var stateDist =
            from maze in InitialMazeDist(grid)
            select new BinaryTreeState(Initial: maze, Steps: [], Current: maze);

        foreach (var cell in grid.Cells)
        {
            stateDist = stateDist.SelectMany(state =>
                from maze in NextStepDist(state.Current, grid, cell)
                select state with
                {
                    Steps = state.Steps.Add(new BinaryTreeStep(cell, maze)),
                    Current = maze
                });
        }

        return stateDist;
    }

    private static IDistribution<Maze> InitialMazeDist(Grid grid)
    {
        var initialState = Maze.WithAllWalls(grid);

        return Singleton.New(initialState);
    }

    private static IDistribution<Maze> NextStepDist(Maze maze, Grid grid, Cell cell)
    {
        var validDirections = GetValidDirections(cell, grid);

        if (UniformDistribution.TryCreate(validDirections, out var directionDist))
        {
            return directionDist.Select(dir => maze.RemoveWall(cell, dir));
        }
        else
        {
            return Singleton.New(maze);
        }
    }

    private static IEnumerable<Direction> GetValidDirections(Cell cell, Grid grid) =>
        new[] { Direction.South, Direction.East }.Where(dir => grid.CanRemoveWall(cell, dir));

    private record BinaryTreeState(Maze Initial, ImmutableList<BinaryTreeStep> Steps, Maze Current);
}