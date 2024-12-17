namespace ProcGenFun.Mazes;

using System.Collections.Immutable;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class Sidewinder
{
    public static IDistribution<Maze> MazeDistribution(Grid grid)
    {
        var initialState = new State(Maze.WithAllWalls(grid));

        IDistribution<State> stateDist = Singleton.New(initialState);

        foreach (var y in grid.RowIndices)
        {
            stateDist = RowDist(stateDist, grid, y);
        }

        return stateDist.Select(s => s.Maze);
    }

    private static IDistribution<State> RowDist(IDistribution<State> stateDist, Grid grid, int y)
    {
        foreach (var x in grid.ColumnIndices)
        {
            var cell = new Cell(x, y);
            stateDist = CellDist(stateDist, grid, cell);
        }

        return stateDist;
    }

    private static IDistribution<State> CellDist(IDistribution<State> stateDist, Grid grid, Cell cell)
    {
        // take from state dist
        // calculate valid actions
        // select one and apply it, or do nothing
    }

    private static IEnumerable<Action> GetValidActions(Grid grid, Cell cell)
    {
        if (grid.AdjacentCellOrNull(cell, Direction.East) != null)
        {
            yield return Action.CarveEast;
        }

        if (grid.AdjacentCellOrNull(cell, Direction.North) != null)
        {
            yield return Action.CloseRun;
        }
    }

    private record State(Maze Maze, ImmutableList<Cell> CurrentRun);

    private enum Action { CarveEast, CloseRun }
}
