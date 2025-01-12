namespace ProcGenFun.Mazes;

using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;
using System.Diagnostics.CodeAnalysis;

public static class RecursiveBacktracker
{
    public static IDistribution<History<RBState>> HistoryDist(Grid grid) => new RecursiveBacktrackerMazeDist(grid);

    private class RecursiveBacktrackerMazeDist : IDistribution<History<RBState>>
    {
        private Grid grid;

        public RecursiveBacktrackerMazeDist(Grid grid)
        {
            this.grid = grid;
        }

        public History<RBState> Sample<TRng>(TRng rng) where TRng : notnull, IRng
        {
            var initialState = InitialStateDist().Sample(rng);

            var history = new History<RBState>([], Current: initialState);

            while (!ShouldStop(history.Current))
            {
                var nextState = NextStepDist(history.Current).Sample(rng);

                history = new History<RBState>(history.Previous.Add(history.Current), nextState);
            }

            return history;
        }

        private IDistribution<RBState> InitialStateDist() =>
            from initialCell in UniformDistribution.CreateOrThrow(this.grid.Cells)
            select
                new RBState(
                    Maze: Maze.WithAllWalls(this.grid),
                    CurrentCell: initialCell,
                    Stack: [],
                    Visited: [initialCell]);

        private bool ShouldStop(RBState state) => state.Visited.Count == this.grid.Cells.Count() && state.Stack.IsEmpty;

        private IDistribution<RBState> NextStepDist(RBState state)
        {
            var neighbouringDirections = this.grid.NeighbouringDirections(state.CurrentCell)
                    .Where(d => !state.Visited.Contains(this.grid.AdjacentCellOrNull(state.CurrentCell, d)!));

            if (UniformDistribution.TryCreate(neighbouringDirections, out var directionDist))
            {
                return
                    from direction in directionDist
                    let ccc = this.grid.AdjacentCellOrNull(state.CurrentCell, direction)!
                    select
                        new RBState(
                            Maze: state.Maze.RemoveWall(state.CurrentCell, direction),
                            CurrentCell: ccc,
                            Stack: state.Stack.Push(state.CurrentCell),
                            Visited: state.Visited.Add(ccc));
            }
            else
            {
                var foo = state.Stack.Pop(out var bar);
                return Singleton.New(state with { CurrentCell = bar, Stack = foo });
            }
        }

        public bool TrySample<TRng>(TRng rng, [MaybeNullWhen(false)] out History<RBState> result) where TRng : notnull, IRng
        {
            result = Sample(rng);
            return true;
        }
    }
}
