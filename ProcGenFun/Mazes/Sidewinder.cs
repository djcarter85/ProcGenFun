namespace ProcGenFun.Mazes;

using System.Collections.Immutable;
using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class Sidewinder
{
    public static IDistribution<Maze<Cell>> MazeDist(Grid grid) =>
        HistoryDist(grid).Select(h => h.Current);

    public static IDistribution<SidewinderHistory> HistoryDist(Grid grid)
    {
        var initialMaze = Maze<Cell>.WithNoEdges(grid.Cells);

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
        var initialState = new RowState(Maze: history.Current, RunBeforeWallRemoved: [], Run: []);

        IDistribution<RollingRowState> rollingStateDist =
            Singleton.New(new RollingRowState(Previous: [], Current: initialState));

        foreach (var x in grid.ColumnIndices)
        {
            rollingStateDist =
                from rollingState in rollingStateDist
                from state in CellDist(rollingState.Current, grid, new Cell(x, y))
                select new RollingRowState(rollingState.Previous.Add(state), state);
        }

        return
            from rollingState in rollingStateDist
            select new SidewinderHistory(
                history.Initial,
                history.Steps.AddRange(
                    rollingState.Previous.Select(
                        x => new SidewinderStep(Maze: x.Maze, RunBeforeWallRemoved: x.RunBeforeWallRemoved, Run: x.Run))),
                rollingState.Current.Maze);
    }

    private static IDistribution<RowState> CellDist(RowState rowState, Grid grid, Cell cell)
    {
        var runBeforeWallRemoved = rowState.Run.Add(cell);
        var validActions = GetValidActions(grid, cell);
        if (UniformDistribution.TryCreate(validActions, out var actionDist))
        {
            Maze<Cell> maze = rowState.Maze;
            return
                from action in actionDist
                from newRowState in ApplyAction(maze, runBeforeWallRemoved, cell, action, grid)
                select newRowState;
        }
        else
        {
            return Singleton.New(rowState with { RunBeforeWallRemoved = runBeforeWallRemoved, Run = runBeforeWallRemoved });
        }
    }

    private static IDistribution<RowState> ApplyAction(
        Maze<Cell> maze, ImmutableList<Cell> runBeforeWallRemoved, Cell cell, Action action, Grid grid) =>
        action switch
        {
            Action.RemoveEastWall => Singleton.New(RemoveEastWall(maze, runBeforeWallRemoved, cell, grid)),
            Action.CloseRun => CloseRunDist(maze, runBeforeWallRemoved, grid),
            _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
        };

    private static RowState RemoveEastWall(
        Maze<Cell> maze, ImmutableList<Cell> runBeforeWallRemoved, Cell cell, Grid grid) =>
        new RowState(
            Maze: maze.RemoveWall(grid, cell, Direction.East),
            RunBeforeWallRemoved: runBeforeWallRemoved,
            Run: runBeforeWallRemoved);

    private static IDistribution<RowState> CloseRunDist(
        Maze<Cell> maze, ImmutableList<Cell> runBeforeWallRemoved, Grid grid) =>
        from cellToRemoveSouthWallFrom in UniformDistribution.Create(runBeforeWallRemoved)
        select new RowState(
            Maze: maze.RemoveWall(grid, cellToRemoveSouthWallFrom, Direction.South),
            RunBeforeWallRemoved: runBeforeWallRemoved,
            Run: []);

    private static IEnumerable<Action> GetValidActions(Grid grid, Cell cell)
    {
        if (grid.AdjacentCellExists(cell, Direction.East))
        {
            yield return Action.RemoveEastWall;
        }

        if (grid.AdjacentCellExists(cell, Direction.South))
        {
            yield return Action.CloseRun;
        }
    }

    private record RowState(Maze<Cell> Maze, ImmutableList<Cell> RunBeforeWallRemoved, ImmutableList<Cell> Run);

    private record RollingRowState(ImmutableList<RowState> Previous, RowState Current);

    private enum Action { RemoveEastWall, CloseRun }
}
