namespace ProcGenFun;

using System.Collections.Immutable;

public class Maze<TVertex> where TVertex : notnull
{
    private readonly ImmutableDictionary<TVertex, ImmutableList<TVertex>> adjacencyMatrix;

    private Maze(ImmutableDictionary<TVertex, ImmutableList<TVertex>> adjacencyMatrix)
    {
        this.adjacencyMatrix = adjacencyMatrix;
    }

    public static Maze<TVertex> WithNoEdges(IEnumerable<TVertex> vertices) =>
        new(
            vertices.Aggregate(
                ImmutableDictionary<TVertex, ImmutableList<TVertex>>.Empty,
                (current, vertex) => current.Add(vertex, [])));

    public bool EdgeExistsBetween(TVertex vertex1, TVertex vertex2) =>
        this.adjacencyMatrix[vertex1].Contains(vertex2);

    public Maze<TVertex> AddEdge(TVertex vertex1, TVertex vertex2) =>
        new(
            this.adjacencyMatrix
                .SetItem(vertex1, this.adjacencyMatrix[vertex1].Add(vertex2))
                .SetItem(vertex2, this.adjacencyMatrix[vertex2].Add(vertex1)));
}

public static class Maze
{
    public static Maze<TVertex> WithNoEdges<TVertex>(IEnumerable<TVertex> vertices)
        where TVertex : notnull =>
        Maze<TVertex>.WithNoEdges(vertices);
}
