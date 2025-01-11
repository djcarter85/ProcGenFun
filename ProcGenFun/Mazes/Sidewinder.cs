namespace ProcGenFun.Mazes;

using System.Collections.Immutable;
using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class Sidewinder
{
    public static IDistribution<Maze> MazeDist(Grid grid) =>
        HistoryDist(grid).Select(h => h.Current);

    public static IDistribution<SidewinderHistory> HistoryDist(Grid grid)
    {
        var initialMaze = Maze.WithAllWalls(grid);

        IDistribution<SidewinderHistory> historyDist =
            Singleton.New(new SidewinderHistory(Initial: initialMaze, Steps: [], Current: initialMaze));

        foreach (var y in grid.RowIndices)
        {
            historyDist = historyDist.SelectMany(h => RowDist(h, grid, y));
        }

        return historyDist;
    }

    private static IDistribution<SidewinderHistory> RowDist(SidewinderHistory history, Grid grid, int y)
    {
        var initialState = new RowState(history.Current, Run: []);

        IDistribution<RollingRowState> rollingStateDist = Singleton.New(
            new RollingRowState(Previous: [], Current: initialState));

        foreach (var x in grid.ColumnIndices)
        {
            var cell = new Cell(x, y);

            rollingStateDist =
                from rollingState in rollingStateDist
                from state in CellDist(rollingState.Current, grid, cell)
                select new RollingRowState(
                    // TODO: it would be nice if CellDist could also return us how the run looked before removing the wall
                    rollingState.Previous.Add(rollingState.Current with { Run = rollingState.Current.Run.Add(cell) }),
                    state);
        }

        return rollingStateDist.Select(
            rollingState => new SidewinderHistory(
                history.Initial,
                history.Steps.AddRange(rollingState.Previous.Select(x => new SidewinderStep(x.Maze, x.Run))),
                rollingState.Current.Maze));
    }

    private static IDistribution<RowState> CellDist(RowState rowState, Grid grid, Cell cell)
    {
        if (UniformDistribution.TryCreate(GetValidActions(grid, cell), out var actionDist))
        {
            return actionDist.SelectMany(a => a switch
            {
                Action.RemoveEastWall => RemoveEastWallDist(rowState, cell),
                Action.CloseRun => CloseRunDist(rowState, cell),
                _ => throw new ArgumentOutOfRangeException(nameof(a), a, null)
            });
        }
        else
        {
            return Singleton.New(rowState with { Run = rowState.Run.Add(cell) });
        }
    }

    private static IDistribution<RowState> RemoveEastWallDist(RowState rowState, Cell cell) =>
        Singleton.New(RemoveEastWall(rowState, cell));

    private static RowState RemoveEastWall(RowState rowState, Cell cell) =>
        new(Maze: rowState.Maze.RemoveWall(cell, Direction.East), Run: rowState.Run.Add(cell));

    private static IDistribution<RowState> CloseRunDist(RowState rowState, Cell cell) =>
        from cellToRemoveSouthWallFrom in UniformDistribution.CreateOrThrow(rowState.Run.Add(cell))
        select CloseRun(rowState, cellToRemoveSouthWallFrom);

    private static RowState CloseRun(RowState rowState, Cell cellToRemoveSouthWallFrom) =>
        new(rowState.Maze.RemoveWall(cellToRemoveSouthWallFrom, Direction.South), []);

    private static IEnumerable<Action> GetValidActions(Grid grid, Cell cell)
    {
        if (grid.AdjacentCellOrNull(cell, Direction.East) != null)
        {
            yield return Action.RemoveEastWall;
        }

        if (grid.AdjacentCellOrNull(cell, Direction.South) != null)
        {
            yield return Action.CloseRun;
        }
    }

    private record RowState(Maze Maze, ImmutableList<Cell> Run);

    private record RollingRowState(ImmutableList<RowState> Previous, RowState Current)
    {
        public ImmutableList<RowState> All => this.Previous.Add(this.Current);
    }

    private enum Action { RemoveEastWall, CloseRun }
}
