namespace ProcGenFun;

using System.Collections.Immutable;
using ProcGenFun.Mazes;

public class Maze
{
    private readonly ImmutableSortedDictionary<Cell, ImmutableList<Cell>> cellWalls;

    private Maze(Grid grid, ImmutableSortedDictionary<Cell, ImmutableList<Cell>> cellWalls)
    {
        this.Grid = grid;
        this.cellWalls = cellWalls;
    }

    public Grid Grid { get; }

    public IEnumerable<Cell> Cells => this.cellWalls.Keys;

    public static Maze WithAllWalls(Grid grid)
    {
        var cellWalls = ImmutableSortedDictionary<Cell, ImmutableList<Cell>>.Empty
            .WithComparers(Cell.Comparer);

        foreach (var cell in grid.Cells)
        {
            cellWalls = cellWalls.Add(cell, []);
        }

        return new Maze(grid, cellWalls);
    }

    public bool WallExists(Cell cell, Cell cell2) => !this.cellWalls[cell].Contains(cell2);

    public Maze RemoveWall(Cell cell, Cell cell2)
    {
        return new Maze(
            this.Grid,
            this.cellWalls
                .SetItem(cell, this.cellWalls[cell].Add(cell2))
                .SetItem(cell2, this.cellWalls[cell2].Add(cell)));
    }
}
