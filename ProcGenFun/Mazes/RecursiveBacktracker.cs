namespace ProcGenFun.Mazes;

using System.Collections.Immutable;
using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class RecursiveBacktracker
{
    public static IDistribution<Maze<T>> MazeDist<T>(
        IReadOnlyList<T> vertices, Func<T, IEnumerable<T>> getNeighbours) where T : notnull =>
        from history in HistoryDist(vertices, getNeighbours)
        select history.Last().Maze;

    public static IDistribution<IReadOnlyList<RecursiveBacktrackerState<T>>> HistoryDist<T>(
        IReadOnlyList<T> vertices, Func<T, IEnumerable<T>> getNeighbours) where T : notnull =>
        from initial in InitialStateDist(vertices)
        from randomWalk in RandomWalk.New(initial, s => NextStateDist(s, getNeighbours))
        select randomWalk.TakeWhile(s => !StopIteration(s, vertices)).ToReadOnly();

    private static bool StopIteration<T>(RecursiveBacktrackerState<T> state, IReadOnlyList<T> vertices)
        where T : notnull =>
        AllVerticesVisited(state, vertices) && state.Path.IsEmpty;

    private static bool AllVerticesVisited<T>(RecursiveBacktrackerState<T> state, IReadOnlyList<T> vertices)
        where T : notnull =>
        state.Visited.Count == vertices.Count;

    private static IDistribution<RecursiveBacktrackerState<T>> InitialStateDist<T>(IReadOnlyList<T> vertices)
        where T : notnull =>
        from vertex in UniformDistribution.Create(vertices)
        select new RecursiveBacktrackerState<T>(
            Maze: Maze.WithNoEdges(vertices),
            CurrentVertex: vertex,
            Path: [],
            Visited: [vertex]);

    private static IDistribution<RecursiveBacktrackerState<T>> NextStateDist<T>(
        RecursiveBacktrackerState<T> state, Func<T, IEnumerable<T>> getNeighbours)
        where T : notnull
    {
        var unvisitedNeighbours = getNeighbours(state.CurrentVertex)
            .Where(n => !state.Visited.Contains(n));
        return UniformDistribution.TryCreate(unvisitedNeighbours, out var unvisitedNeighbourDist) ?
            ProceedDist(state, unvisitedNeighbourDist) :
            Singleton.New(Backtrack(state));
    }

    private static IDistribution<RecursiveBacktrackerState<T>> ProceedDist<T>(
        RecursiveBacktrackerState<T> state, IDistribution<T> unvisitedNeighbourDist)
        where T : notnull =>
        from neighbour in unvisitedNeighbourDist
        select new RecursiveBacktrackerState<T>(
            Maze: state.Maze.AddEdge(state.CurrentVertex, neighbour),
            CurrentVertex: neighbour,
            Path: state.Path.Push(state.CurrentVertex),
            Visited: state.Visited.Add(neighbour));

    private static RecursiveBacktrackerState<T> Backtrack<T>(RecursiveBacktrackerState<T> state)
        where T : notnull =>
        state with { Path = state.Path.Pop(out var vertex), CurrentVertex = vertex };
}