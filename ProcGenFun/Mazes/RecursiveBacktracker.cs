namespace ProcGenFun.Mazes;

using System.Collections.Immutable;
using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class RecursiveBacktracker
{
    public static IDistribution<Maze> MazeDist(Grid grid) =>
        from history in HistoryDist(grid)
        select history.Last().Maze;

    public static IDistribution<IReadOnlyList<RecursiveBacktrackerState>> HistoryDist(Grid grid) =>
        from initial in InitialStateDist(grid)
        from randomWalk in RandomWalk.New(initial, s => NextStateDist(grid, s))
        select randomWalk.TakeWhile(s => !s.Path.IsEmpty).ToReadOnly();

    private static IDistribution<RecursiveBacktrackerState> InitialStateDist(Grid grid) =>
        from cell in UniformDistribution.Create(grid.Cells)
        select new RecursiveBacktrackerState(
            Maze: Maze.WithAllWalls(grid),
            CurrentCell: cell,
            Path: ImmutableStack<Cell>.Empty.Push(cell),
            Visited: ImmutableHashSet<Cell>.Empty.Add(cell));

    private static IDistribution<RecursiveBacktrackerState> NextStateDist(Grid grid, RecursiveBacktrackerState state)
    {
        var unvisitedNeighbours = grid.GetNeighbours(state.CurrentCell)
            .Where(n => !state.Visited.Contains(n.Cell));
        return UniformDistribution.TryCreate(unvisitedNeighbours, out var unvisitedNeighbourDist) ?
            ProceedDist(state, unvisitedNeighbourDist) :
            Singleton.New(Backtrack(state));
    }

    private static IDistribution<RecursiveBacktrackerState> ProceedDist(
        RecursiveBacktrackerState state, IDistribution<Neighbour> nextCellDist) =>
        from neighbour in nextCellDist
        select new RecursiveBacktrackerState(
            Maze: state.Maze.RemoveWall(state.CurrentCell, neighbour.Direction),
            CurrentCell: neighbour.Cell,
            Path: state.Path.Push(neighbour.Cell),
            Visited: state.Visited.Add(neighbour.Cell));

    private static RecursiveBacktrackerState Backtrack(RecursiveBacktrackerState state) =>
        state with { Path = state.Path.Pop(out var cell), CurrentCell = cell };
}