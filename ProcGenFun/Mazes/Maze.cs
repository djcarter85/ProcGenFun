namespace ProcGenFun;

using System.Collections.Immutable;
using ProcGenFun.Mazes;

public class Maze
{
    private readonly ImmutableDictionary<Cell, ImmutableList<Cell>> adjacencyMatrix;

    private Maze(ImmutableDictionary<Cell, ImmutableList<Cell>> adjacencyMatrix)
    {
        this.adjacencyMatrix = adjacencyMatrix;
    }

    public static Maze WithNoEdges(IEnumerable<Cell> cells) =>
        new(
            cells.Aggregate(
                ImmutableDictionary<Cell, ImmutableList<Cell>>.Empty,
                (current, cell) => current.Add(cell, [])));

    public bool EdgeExistsBetween(Cell cell1, Cell cell2) => this.adjacencyMatrix[cell1].Contains(cell2);

    public Maze AddEdge(Cell cell1, Cell cell2) =>
        new(
            this.adjacencyMatrix
                .SetItem(cell1, this.adjacencyMatrix[cell1].Add(cell2))
                .SetItem(cell2, this.adjacencyMatrix[cell2].Add(cell1)));
}
