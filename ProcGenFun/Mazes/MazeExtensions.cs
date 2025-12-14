namespace ProcGenFun.Mazes;

public static class MazeExtensions
{
    public static Maze RemoveWall(this Maze maze, Grid grid, Cell cell, Direction direction)
    {
        var adjacentCell = grid.AdjacentCellOrNull(cell, direction);

        if (adjacentCell == null)
        {
            throw new InvalidOperationException($"Cell does not have an adjacent cell to the {direction}");
        }

        return maze.AddEdge(cell, adjacentCell);
    }

    public static bool WallExists(this Maze maze, Cell cell, Direction direction, Grid grid)
    {
        var adjacentCell = grid.AdjacentCellOrNull(cell, direction);

        if (adjacentCell == null)
        {
            // If there isn't an adjacent cell, then it must mean we're at the edge of the maze, and walls exist around
            // the entire boundary.
            return true;
        }

        return !maze.EdgeExistsBetween(cell, adjacentCell);
    }
}