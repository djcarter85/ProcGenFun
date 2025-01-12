namespace ProcGenFun.Mazes;

using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;
using System.Diagnostics.CodeAnalysis;

public static class AldousBroder
{
    public static IDistribution<History<ABState>> HistoryDist(Grid grid) => new AldousBroderHistoryDistribution(grid);

    private class AldousBroderHistoryDistribution : IDistribution<History<ABState>>
    {
        private Grid grid;

        public AldousBroderHistoryDistribution(Grid grid)
        {
            this.grid = grid;
        }

        public History<ABState> Sample<TRng>(TRng rng) where TRng : notnull, IRng
        {
            var initialState = InitialStateDist().Sample(rng);

            var history = new History<ABState>([], Current: initialState);

            while (!ShouldStop(history.Current))
            {
                var nextState = NextStepDist(history.Current).Sample(rng);

                history = new History<ABState>(history.Previous.Add(history.Current), nextState);
            }

            return history;
        }

        private IDistribution<ABState> InitialStateDist() =>
            from initialCell in UniformDistribution.CreateOrThrow(this.grid.Cells)
            select
                new ABState(
                    Maze: Maze.WithAllWalls(this.grid),
                    CurrentCell: initialCell,
                    Visited: [initialCell]);

        private bool ShouldStop(ABState state) => state.Visited.Count == this.grid.Cells.Count();

        private IDistribution<ABState> NextStepDist(ABState state)
        {
            var neighbouringDirections = this.grid.NeighbouringDirections(state.CurrentCell);

            var directionDist = UniformDistribution.CreateOrThrow(neighbouringDirections);

            return
                from direction in directionDist
                let newCell = this.grid.AdjacentCellOrNull(state.CurrentCell, direction)!
                let alreadyVisitedNewCell = state.Visited.Contains(newCell)
                select
                    alreadyVisitedNewCell ?
                    state with { CurrentCell = newCell } :
                    new ABState(
                        Maze: state.Maze.RemoveWall(state.CurrentCell, direction),
                        CurrentCell: newCell,
                        Visited: state.Visited.Add(newCell));

        }

        public bool TrySample<TRng>(TRng rng, [MaybeNullWhen(false)] out History<ABState> result) where TRng : notnull, IRng
        {
            result = Sample(rng);
            return true;
        }
    }
}
