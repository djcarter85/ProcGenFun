namespace ProcGenFun.Mazes;

using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

public static class RecursiveBacktracker
{
    public static IDistribution<Maze> MazeDist(Grid grid) =>
        from state in new RecursiveBacktrackerDist<State>(
            InitialStateDist(grid),
            StopConditionDist,
            s => NextStateDist(grid, s))
        select state.Maze;

    private static IDistribution<State> InitialStateDist(Grid grid) =>
        from cell in UniformDistribution.Create(grid.Cells)
        select new State(
            Maze: Maze.WithAllWalls(grid),
            CurrentCell: cell,
            Path: ImmutableStack<Cell>.Empty.Push(cell),
            Visited: ImmutableHashSet<Cell>.Empty.Add(cell));

    private static IDistribution<bool> StopConditionDist(State state) => Singleton.New(state.Path.IsEmpty);

    private static IDistribution<State> NextStateDist(Grid grid, State state)
    {
        // todo mix of null checks and directions, not great
        var adjacentUnvisitedCellDirections = grid.AdjacentDirections(state.CurrentCell)
            .Where(d => !state.Visited.Contains(grid.AdjacentCellOrNull(state.CurrentCell, d)!));
        if (UniformDistribution.TryCreate(adjacentUnvisitedCellDirections, out var adjacentDirectionDist))
        {
            return
                from direction in adjacentDirectionDist
                let cell = grid.AdjacentCellOrNull(state.CurrentCell, direction)
                select new State(
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

    // todo more general name and extract
    // maybe a helper function for creating?
    private class RecursiveBacktrackerDist<T> : IDistribution<T>
    {
        private readonly IDistribution<T> initialStateDist;
        private readonly Func<T, IDistribution<bool>> stopConditionDist;
        private readonly Func<T, IDistribution<T>> nextStateDist;

        public RecursiveBacktrackerDist(
            IDistribution<T> initialStateDist,
            Func<T, IDistribution<bool>> stopConditionDist,
            Func<T, IDistribution<T>> nextStateDist)
        {
            this.initialStateDist = initialStateDist;
            this.stopConditionDist = stopConditionDist;
            this.nextStateDist = nextStateDist;
        }

        public T Sample<TRng>(TRng rng)
            where TRng : notnull, IRng
        {
            var state = this.initialStateDist.Sample(rng);

            while (!stopConditionDist(state).Sample(rng))
            {
                state = nextStateDist(state).Sample(rng);
            }

            return state;
        }

        public bool TrySample<TRng>(TRng rng, [MaybeNullWhen(false)] out T result)
            where TRng : notnull, IRng
        {
            throw new NotImplementedException();
        }
    }

    private record State(Maze Maze, Cell CurrentCell, ImmutableStack<Cell> Path, ImmutableHashSet<Cell> Visited);
}
