namespace ProcGenFun.Mazes;

using System.Collections.Immutable;
using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class RecursiveBacktracker
{
    public static IDistribution<Maze<T>> MazeDist<T>(
        IReadOnlyList<T> cells, Func<T, IEnumerable<T>> getNeighbours) where T : notnull =>
        from history in HistoryDist(cells, getNeighbours)
        select history.Last().Maze;

    public static IDistribution<IReadOnlyList<RecursiveBacktrackerState<T>>> HistoryDist<T>(
        IReadOnlyList<T> cells, Func<T, IEnumerable<T>> getNeighbours) where T : notnull =>
        from initial in InitialStateDist(cells)
        from randomWalk in RandomWalk.New(initial, s => NextStateDist(s, getNeighbours))
        select randomWalk.TakeWhile(s => !StopIteration(s, cells)).ToReadOnly();

    private static bool StopIteration<T>(RecursiveBacktrackerState<T> state, IReadOnlyList<T> cells) => 
        AllCellsVisited(state, cells) && state.Path.IsEmpty;

    private static bool AllCellsVisited<T>(RecursiveBacktrackerState<T> state, IReadOnlyList<T> cells) =>
        state.Visited.Count == cells.Count;

    private static IDistribution<RecursiveBacktrackerState<T>> InitialStateDist<T>(IReadOnlyList<T> cells)
        where T : notnull =>
        from cell in UniformDistribution.Create(cells)
        select new RecursiveBacktrackerState<T>(
            Maze: Maze.WithNoEdges(cells),
            CurrentCell: cell,
            Path: [],
            Visited: [cell]);

    private static IDistribution<RecursiveBacktrackerState<T>> NextStateDist<T>(
        RecursiveBacktrackerState<T> state, Func<T, IEnumerable<T>> getNeighbours)
    {
        var unvisitedNeighbours = getNeighbours(state.CurrentCell)
            .Where(n => !state.Visited.Contains(n));
        return UniformDistribution.TryCreate(unvisitedNeighbours, out var unvisitedNeighbourDist) ?
            ProceedDist(state, unvisitedNeighbourDist) :
            Singleton.New(Backtrack(state));
    }

    private static IDistribution<RecursiveBacktrackerState<T>> ProceedDist<T>(
        RecursiveBacktrackerState<T> state, IDistribution<T> unvisitedNeighbourDist) =>
        from neighbour in unvisitedNeighbourDist
        select new RecursiveBacktrackerState<T>(
            Maze: state.Maze.AddEdge(state.CurrentCell, neighbour),
            CurrentCell: neighbour,
            Path: state.Path.Push(state.CurrentCell),
            Visited: state.Visited.Add(neighbour));

    private static RecursiveBacktrackerState<T> Backtrack<T>(RecursiveBacktrackerState<T> state) =>
        state with { Path = state.Path.Pop(out var cell), CurrentCell = cell };
}