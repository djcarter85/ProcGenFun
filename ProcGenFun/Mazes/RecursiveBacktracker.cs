namespace ProcGenFun.Mazes;

using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

public static class RecursiveBacktracker
{
    public static IDistribution<History> HistoryDist(Grid grid) => new RecursiveBacktrackerMazeDist(grid);

    private class RecursiveBacktrackerMazeDist : IDistribution<History>
    {
        private Grid grid;

        public RecursiveBacktrackerMazeDist(Grid grid)
        {
            this.grid = grid;
        }

        public History Sample<TRng>(TRng rng) where TRng : notnull, IRng
        {
            var initialState = InitialStateDist().Sample(rng);

            var history = new History([], Current: initialState);

            while (!ShouldStop(history.Current))
            {
                var nextState = NextStepDist(history.Current).Sample(rng);

                history = new History(history.Previous.Add(history.Current), nextState);
            }

            return history;
        }

        private IDistribution<State> InitialStateDist() =>
            from initialCell in UniformDistribution.CreateOrThrow(this.grid.Cells)
            select
                new State(
                    Maze: Maze.WithAllWalls(this.grid),
                    CurrentCell: initialCell,
                    Stack: [initialCell],
                    Visited: [initialCell]);

        private static bool ShouldStop(State state) => state.Stack.IsEmpty;

        private IDistribution<State> NextStepDist(State state)
        {
            var neighbouringDirections = this.grid.NeighbouringDirections(state.CurrentCell)
                    .Where(d => !state.Visited.Contains(this.grid.AdjacentCellOrNull(state.CurrentCell, d)!));

            if (UniformDistribution.TryCreate(neighbouringDirections, out var directionDist))
            {
                return
                    from direction in directionDist
                    let ccc = this.grid.AdjacentCellOrNull(state.CurrentCell, direction)!
                    select
                        new State(
                            Maze: state.Maze.RemoveWall(state.CurrentCell, direction),
                            CurrentCell: ccc,
                            Stack: state.Stack.Push(ccc),
                            Visited: state.Visited.Add(ccc));
            }
            else
            {
                var foo = state.Stack.Pop(out var bar);
                return Singleton.New(state with { CurrentCell = bar, Stack = foo });
            }
        }

        public bool TrySample<TRng>(TRng rng, [MaybeNullWhen(false)] out History result) where TRng : notnull, IRng
        {
            result = Sample(rng);
            return true;
        }
    }
}

public record State(Maze Maze, Cell CurrentCell, ImmutableStack<Cell> Stack, ImmutableList<Cell> Visited);

public record History(ImmutableList<State> Previous, State Current);
