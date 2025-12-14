namespace ProcGenFun;

using System.Collections.Immutable;

public class Maze<TVertex> where TVertex : notnull
{
    private readonly ImmutableDictionary<TVertex, ImmutableList<TVertex>> adjacencyMatrix;

    private Maze(ImmutableDictionary<TVertex, ImmutableList<TVertex>> adjacencyMatrix)
    {
        this.adjacencyMatrix = adjacencyMatrix;
    }

    public static Maze<TVertex> WithNoEdges(IEnumerable<TVertex> cells) =>
        new(
            cells.Aggregate(
                ImmutableDictionary<TVertex, ImmutableList<TVertex>>.Empty,
                (current, cell) => current.Add(cell, [])));

    public bool EdgeExistsBetween(TVertex cell1, TVertex cell2) => this.adjacencyMatrix[cell1].Contains(cell2);

    public Maze<TVertex> AddEdge(TVertex cell1, TVertex cell2) =>
        new(
            this.adjacencyMatrix
                .SetItem(cell1, this.adjacencyMatrix[cell1].Add(cell2))
                .SetItem(cell2, this.adjacencyMatrix[cell2].Add(cell1)));
}
