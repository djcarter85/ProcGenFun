namespace ProcGenFun.Mazes;

using System.Collections.Immutable;
using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class Sidewinder
{
    public static IDistribution<Maze> MazeDist(Grid grid)
    {
        var initialMaze = Maze.WithAllWalls(grid);

        IDistribution<Maze> mazeDist = Singleton.New(initialMaze);

        foreach (var y in grid.RowIndices)
        {
            mazeDist = mazeDist.SelectMany(m => RowDist(m, grid, y));
        }

        return mazeDist;
    }

    private static IDistribution<Maze> RowDist(Maze maze, Grid grid, int y)
    {
        var initialState = new RowState(maze, Run: []);

        IDistribution<RowState> stateDist = Singleton.New(initialState);

        foreach (var x in grid.ColumnIndices)
        {
            stateDist = stateDist.SelectMany(s => CellDist(s, grid, new Cell(x, y)));
        }

        return stateDist.Select(s => s.Maze);
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

    private enum Action { RemoveEastWall, CloseRun }
}
