namespace ProcGenFun;

using System.Collections.Immutable;
using ProcGenFun.Mazes;

public class Maze
{
    private readonly ImmutableSortedDictionary<Cell, ImmutableList<Cell>> adjacencyMatrix;

    private Maze(Grid grid, ImmutableSortedDictionary<Cell, ImmutableList<Cell>> adjacencyMatrix)
    {
        this.Grid = grid;
        this.adjacencyMatrix = adjacencyMatrix;
    }

    public Grid Grid { get; }

    public IEnumerable<Cell> Cells => this.adjacencyMatrix.Keys;

    public static Maze WithAllWalls(Grid grid) =>
        new(
            grid,
            grid.Cells.Aggregate(
                ImmutableSortedDictionary<Cell, ImmutableList<Cell>>.Empty.WithComparers(Cell.Comparer),
                (current, cell) => current.Add(cell, [])));

    public bool WallExists(Cell cell, Cell cell2) => !this.adjacencyMatrix[cell].Contains(cell2);

    public Maze RemoveWall(Cell cell, Cell cell2)
    {
        return new Maze(
            this.Grid,
            this.adjacencyMatrix
                .SetItem(cell, this.adjacencyMatrix[cell].Add(cell2))
                .SetItem(cell2, this.adjacencyMatrix[cell2].Add(cell)));
    }
}
