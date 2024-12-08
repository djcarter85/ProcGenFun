namespace ProcGenFun;

using System.Collections.Immutable;

public class Maze
{
    private readonly ImmutableDictionary<Cell, ImmutableList<Direction>> cellWalls;

    private Maze(Grid grid, ImmutableDictionary<Cell, ImmutableList<Direction>> cellWalls)
    {
        this.Grid = grid;
        this.cellWalls = cellWalls;
    }

    public Grid Grid { get; }

    public static Maze WithAllWalls(Grid grid)
    {
        var cellWalls = ImmutableDictionary<Cell, ImmutableList<Direction>>.Empty;

        foreach (var cell in grid.Cells)
        {
            cellWalls = cellWalls.Add(cell, [Direction.North, Direction.East, Direction.South, Direction.West]);
        }

        return new Maze(grid, cellWalls);
    }

    public bool WallExists(Cell cell, Direction direction) => this.cellWalls[cell].Contains(direction);
}
