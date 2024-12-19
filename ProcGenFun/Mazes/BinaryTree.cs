namespace ProcGenFun.Mazes;

using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class BinaryTree
{
    public static IDistribution<Maze> MazeDist(Grid grid)
    {
        var mazeDist = InitialMazeDist(grid);

        foreach (var cell in grid.Cells)
        {
            mazeDist = mazeDist.SelectMany(m => NextStepDist(m, grid, cell));
        }

        return mazeDist;
    }

    private static IDistribution<Maze> InitialMazeDist(Grid grid)
    {
        var initialState = Maze.WithAllWalls(grid);

        return Singleton.New(initialState);
    }

    private static IDistribution<Maze> NextStepDist(Maze maze, Grid grid, Cell cell)
    {
        var validDirections = GetValidDirections(cell, grid);

        if (UniformDistribution.TryCreate(validDirections, out var directionDist))
        {
            return directionDist.Select(dir => maze.RemoveWall(cell, dir));
        }
        else
        {
            return Singleton.New(maze);
        }
    }

    private static IEnumerable<Direction> GetValidDirections(Cell cell, Grid grid) =>
        new[] { Direction.South, Direction.East }.Where(dir => grid.CanRemoveWall(cell, dir));
}
