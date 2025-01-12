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
                Stack: [],
                Visited: [initialCell]);

    private static bool ShouldStop(Grid grid, RBState state) => 
        state.Visited.Count == grid.Cells.Count() && state.Stack.IsEmpty;

    private static IDistribution<RBState> NextStepDist(Grid grid, RBState state)
    {
        var neighbouringDirections = grid.NeighbouringDirections(state.CurrentCell)
                .Where(d => !state.Visited.Contains(grid.AdjacentCellOrNull(state.CurrentCell, d)!));

        if (UniformDistribution.TryCreate(neighbouringDirections, out var directionDist))
        {
            return
                from direction in directionDist
                let ccc = grid.AdjacentCellOrNull(state.CurrentCell, direction)!
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
}
