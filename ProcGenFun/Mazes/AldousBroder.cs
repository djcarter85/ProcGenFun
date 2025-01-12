namespace ProcGenFun.Mazes;

using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;
using System.Diagnostics.CodeAnalysis;

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
                Visited: [initialCell]);

    private static bool ShouldStop(Grid grid, ABState state) => state.Visited.Count == grid.Cells.Count();

    private static IDistribution<ABState> NextStepDist(Grid grid, ABState state)
    {
        var neighbouringDirections = grid.NeighbouringDirections(state.CurrentCell);

        var directionDist = UniformDistribution.CreateOrThrow(neighbouringDirections);

        return
            from direction in directionDist
            let newCell = grid.AdjacentCellOrNull(state.CurrentCell, direction)!
            let alreadyVisitedNewCell = state.Visited.Contains(newCell)
            select
                alreadyVisitedNewCell ?
                state with { CurrentCell = newCell } :
                new ABState(
                    Maze: state.Maze.RemoveWall(state.CurrentCell, direction),
                    CurrentCell: newCell,
                    Visited: state.Visited.Add(newCell));

    }
}
