namespace ProcGenFun;

using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class BinaryTree
{
    public static IDistribution<Maze> MazeDistribution(Grid grid)
    {
        var initialState = Maze.WithAllWalls(grid);

        IDistribution<Maze> distribution = Singleton.New(initialState);

        foreach (var cell in grid.Cells)
        {
            var validDirections = GetValidDirections(cell, grid);

            if (validDirections.Any())
            {
                distribution =
                    from maze in distribution
                    from direction in UniformDist(validDirections)
                    select maze.RemoveWall(cell, direction);
            }
        }

        return distribution;
    }

    private static IEnumerable<Direction> GetValidDirections(Cell cell, Grid grid) =>
        from dir in new[] { Direction.North, Direction.East }
        where grid.AdjacentCellOrNull(cell, dir) != null
        select dir;

    private static IDistribution<T> UniformDist<T>(IEnumerable<T> items)
    {
        var list = items.ToList();

        return
            from index in Uniform.NewInclusive(0, list.Count - 1)
            select list[index];
    }
}
