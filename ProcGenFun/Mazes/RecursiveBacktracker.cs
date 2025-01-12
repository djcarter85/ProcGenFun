namespace ProcGenFun.Mazes;

using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class RecursiveBacktracker
{
    public static IDistribution<IEnumerable<RBState>> HistoryDist(Grid grid) =>
        InitialStateDist(grid)
            .Iterate(
                state => Singleton.New(ShouldStop(grid, state)),
                state => NextStepDist(grid, state));

    private static IDistribution<RBState> InitialStateDist(Grid grid) =>
        from initialCell in UniformDistribution.CreateOrThrow(grid.Cells)
        select
            new RBState(
                Maze: Maze.WithAllWalls(grid),
                CurrentCell: initialCell,
                PreviousCells: [],
                VisitedCells: [initialCell]);

    private static bool ShouldStop(Grid grid, RBState state) =>
        state.VisitedCells.Count == grid.Cells.Count() && state.PreviousCells.IsEmpty;

    private static IDistribution<RBState> NextStepDist(Grid grid, RBState state)
    {
        var unvisitedNeighbouringDirections = GetUnvisitedNeighbouringDirections(grid, state);

        if (UniformDistribution.TryCreate(unvisitedNeighbouringDirections, out var directionDist))
        {
            return MoveForward(grid, state, directionDist);
        }
        else
        {
            return RetraceSteps(state);
        }
    }

    private static IEnumerable<Direction> GetUnvisitedNeighbouringDirections(Grid grid, RBState state) =>
        grid.NeighbouringDirections(state.CurrentCell)
            .Where(d => !state.VisitedCells.Contains(grid.AdjacentCellOrNull(state.CurrentCell, d)!));

    private static IDistribution<RBState> MoveForward(
        Grid grid, RBState state, IDistribution<Direction> directionDist) =>
        from direction in directionDist
        let newCurrentCell = grid.AdjacentCellOrNull(state.CurrentCell, direction)!
        select
            new RBState(
                Maze: state.Maze.RemoveWall(state.CurrentCell, direction),
                CurrentCell: newCurrentCell,
                PreviousCells: state.PreviousCells.Push(state.CurrentCell),
                VisitedCells: state.VisitedCells.Add(newCurrentCell));

    private static IDistribution<RBState> RetraceSteps(RBState state) =>
        Singleton.New(
            state with
            {
                PreviousCells = state.PreviousCells.Pop(out var currentCell),
                CurrentCell = currentCell,
            });
}
