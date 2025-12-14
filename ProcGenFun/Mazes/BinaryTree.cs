namespace ProcGenFun.Mazes;

using System.Collections.Immutable;
using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class BinaryTree
{
    public static IDistribution<Maze<RectCell>> MazeDist(RectGrid grid) =>
        from state in StateDist(grid)
        select state.Current;

    public static IDistribution<BinaryTreeHistory> HistoryDist(RectGrid grid) =>
        from state in StateDist(grid)
        select new BinaryTreeHistory(
            Initial: state.Initial,
            Steps: state.Steps,
            Final: state.Current);

    private static IDistribution<BinaryTreeState> StateDist(RectGrid grid)
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

    private static IDistribution<Maze<RectCell>> InitialMazeDist(RectGrid grid)
    {
        var initialState = Maze.WithNoEdges(grid.Cells);

        return Singleton.New(initialState);
    }

    private static IDistribution<Maze<RectCell>> NextStepDist(Maze<RectCell> maze, RectGrid grid, RectCell cell)
    {
        var validDirections = GetValidDirections(cell, grid);

        if (UniformDistribution.TryCreate(validDirections, out var directionDist))
        {
            return directionDist.Select(dir => maze.RemoveWall(grid, cell, dir));
        }
        else
        {
            return Singleton.New(maze);
        }
    }

    private static IEnumerable<RectDirection> GetValidDirections(RectCell cell, RectGrid grid) =>
        new[] { RectDirection.South, RectDirection.East }.Where(dir => grid.CanRemoveWall(cell, dir));

    private record BinaryTreeState(Maze<RectCell> Initial, ImmutableList<BinaryTreeStep> Steps, Maze<RectCell> Current);
}