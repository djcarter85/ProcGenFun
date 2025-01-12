namespace ProcGenFun.Mazes;

using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class AldousBroder
{
    public static IDistribution<IEnumerable<ABState>> HistoryDist(Grid grid) =>
        InitialStateDist(grid)
            .Iterate(
                state => Singleton.New(ShouldStop(grid, state)),
                state => NextStepDist(grid, state));


    private static IDistribution<ABState> InitialStateDist(Grid grid) =>
        from initialCell in UniformDistribution.CreateOrThrow(grid.Cells)
        select
            new ABState(
                Maze: Maze.WithAllWalls(grid),
                CurrentCell: initialCell,
                VisitedCells: [initialCell]);

    private static bool ShouldStop(Grid grid, ABState state) => state.VisitedCells.Count == grid.Cells.Count();

    private static IDistribution<ABState> NextStepDist(Grid grid, ABState state) =>
        from direction in UniformDistribution.CreateOrThrow(grid.NeighbouringDirections(state.CurrentCell))
        let newCurrentCell = grid.AdjacentCellOrNull(state.CurrentCell, direction)!
        let alreadyVisitedNewCurrentCell = state.VisitedCells.Contains(newCurrentCell)
        select
            alreadyVisitedNewCurrentCell ?
            state with { CurrentCell = newCurrentCell } :
            new ABState(
                Maze: state.Maze.RemoveWall(state.CurrentCell, direction),
                CurrentCell: newCurrentCell,
                VisitedCells: state.VisitedCells.Add(newCurrentCell));
}
