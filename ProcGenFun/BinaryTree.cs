namespace ProcGenFun;

using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class BinaryTree
{
    public static IDistribution<Maze> MazeDistribution(Grid grid)
    {
        var initialState = Maze.WithAllWalls(grid);

        IDistribution<Maze> mazeDist = Singleton.New(initialState);

        foreach (var cell in grid.Cells)
        {
            mazeDist = NextStepDistribution(mazeDist, grid, cell);
        }

        return mazeDist;
    }

    private static IDistribution<Maze> NextStepDistribution(
        IDistribution<Maze> mazeDist, Grid grid, Cell cell)
    {
        var validDirections = GetValidDirections(cell, grid);

        return UniformDistribution.TryCreate(validDirections, out var directionDist)
            ? (from maze in mazeDist
                from direction in directionDist
                select maze.RemoveWall(cell, direction))
            : mazeDist;
    }

    private static IEnumerable<Direction> GetValidDirections(Cell cell, Grid grid) =>
        from dir in new[] { Direction.North, Direction.East }
        where grid.AdjacentCellOrNull(cell, dir) != null
        select dir;
}
