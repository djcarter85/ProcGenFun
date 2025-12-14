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
}