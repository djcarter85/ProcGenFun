namespace ProcGenFun.Mazes;

public static class RectMazeExtensions
{
    public static Maze<RectCell> RemoveWall(this Maze<RectCell> maze, RectGrid grid, RectCell cell, RectDirection direction)
    {
        var adjacentCell = grid.AdjacentCellOrNull(cell, direction);

        if (adjacentCell == null)
        {
            throw new InvalidOperationException($"RectCell does not have an adjacent cell to the {direction}");
        }

        return maze.AddEdge(cell, adjacentCell);
    }

    public static bool WallExists(this Maze<RectCell> maze, RectGrid grid, RectCell cell, RectDirection direction)
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