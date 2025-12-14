namespace ProcGenFun.Mazes;

using System.Collections.Immutable;
using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class RecursiveBacktracker
{
    public static IDistribution<Maze<Cell>> MazeDist(
        IReadOnlyList<Cell> cells, Func<Cell, IEnumerable<Cell>> getNeighbours) =>
        from history in HistoryDist(cells, getNeighbours)
        select history.Last().Maze;

    public static IDistribution<IReadOnlyList<RecursiveBacktrackerState>> HistoryDist(
        IReadOnlyList<Cell> cells, Func<Cell, IEnumerable<Cell>> getNeighbours) =>
        from initial in InitialStateDist(cells)
        from randomWalk in RandomWalk.New(initial, s => NextStateDist(s, getNeighbours))
        select randomWalk.TakeWhile(s => !StopIteration(s, cells)).ToReadOnly();

    private static bool StopIteration(RecursiveBacktrackerState state, IReadOnlyList<Cell> cells) => 
        AllCellsVisited(state, cells) && state.Path.IsEmpty;

    private static bool AllCellsVisited(RecursiveBacktrackerState state, IReadOnlyList<Cell> cells) =>
        state.Visited.Count == cells.Count;

    private static IDistribution<RecursiveBacktrackerState> InitialStateDist(IReadOnlyList<Cell> cells) =>
        from cell in UniformDistribution.Create(cells)
        select new RecursiveBacktrackerState(
            Maze: Maze.WithNoEdges(cells),
            CurrentCell: cell,
            Path: [],
            Visited: [cell]);

    private static IDistribution<RecursiveBacktrackerState> NextStateDist(
        RecursiveBacktrackerState state, Func<Cell, IEnumerable<Cell>> getNeighbours)
    {
        var unvisitedNeighbours = getNeighbours(state.CurrentCell)
            .Where(n => !state.Visited.Contains(n));
        return UniformDistribution.TryCreate(unvisitedNeighbours, out var unvisitedNeighbourDist) ?
            ProceedDist(state, unvisitedNeighbourDist) :
            Singleton.New(Backtrack(state));
    }

    private static IDistribution<RecursiveBacktrackerState> ProceedDist(
        RecursiveBacktrackerState state, IDistribution<Cell> unvisitedNeighbourDist) =>
        from neighbour in unvisitedNeighbourDist
        select new RecursiveBacktrackerState(
            Maze: state.Maze.AddEdge(state.CurrentCell, neighbour),
            CurrentCell: neighbour,
            Path: state.Path.Push(state.CurrentCell),
            Visited: state.Visited.Add(neighbour));

    private static RecursiveBacktrackerState Backtrack(RecursiveBacktrackerState state) =>
        state with { Path = state.Path.Pop(out var cell), CurrentCell = cell };
}