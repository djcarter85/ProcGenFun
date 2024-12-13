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
    
    public Maze RemoveWall(Cell cell, Direction direction)
    {
        var adjacentCell = this.Grid.AdjacentCellOrNull(cell, direction);

        if (adjacentCell == null)
        {
            throw new InvalidOperationException($"{direction} wall cannot be removed.");
        }

        return new Maze(
            this.Grid,
            this.cellWalls
                .SetItem(cell, this.cellWalls[cell].Remove(direction))
                .SetItem(adjacentCell, this.cellWalls[adjacentCell].Remove(direction.Opposite())));
    }
}
