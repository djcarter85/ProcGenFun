namespace ProcGenFun.Mazes;

using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;
using System.Collections.Immutable;

public static class RecursiveBacktracker
{
    public static IDistribution<Maze> MazeDist(Grid grid) =>
        from history in HistoryDist(grid)
        select history.Last().Maze;

    public static IDistribution<IReadOnlyList<RecursiveBacktrackerState>> HistoryDist(Grid grid) =>
        from initial in InitialStateDist(grid)
        from randomWalk in RandomWalk.New(initial, s => NextStateDist(grid, s))
        // TODO this might need to also include the last one?
        select (IReadOnlyList<RecursiveBacktrackerState>)randomWalk.TakeWhile(s => !StopCondition(s)).ToList();

    private static IDistribution<RecursiveBacktrackerState> InitialStateDist(Grid grid) =>
        from cell in UniformDistribution.Create(grid.Cells)
        select new RecursiveBacktrackerState(
            Maze: Maze.WithAllWalls(grid),
            CurrentCell: cell,
            Path: ImmutableStack<Cell>.Empty.Push(cell),
            Visited: ImmutableHashSet<Cell>.Empty.Add(cell));

    private static bool StopCondition(RecursiveBacktrackerState state) => state.Path.IsEmpty;

    private static IDistribution<RecursiveBacktrackerState> NextStateDist(Grid grid, RecursiveBacktrackerState state)
    {
        // todo mix of null checks and directions, not great
        var adjacentUnvisitedCellDirections = grid.AdjacentDirections(state.CurrentCell)
            .Where(d => !state.Visited.Contains(grid.AdjacentCellOrNull(state.CurrentCell, d)!));
        if (UniformDistribution.TryCreate(adjacentUnvisitedCellDirections, out var adjacentDirectionDist))
        {
            return
                from direction in adjacentDirectionDist
                let cell = grid.AdjacentCellOrNull(state.CurrentCell, direction)
                select new RecursiveBacktrackerState(
                    Maze: state.Maze.RemoveWall(state.CurrentCell, direction),
                    CurrentCell: cell,
                    Path: state.Path.Push(cell),
                    Visited: state.Visited.Add(cell));
        }
        else
        {
            // todo extract method and make it clearly deterministic
            return Singleton.New(state with { Path = state.Path.Pop(out var cell), CurrentCell = cell });
        }
    }
}