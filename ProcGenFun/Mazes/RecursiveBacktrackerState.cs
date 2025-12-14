namespace ProcGenFun.Mazes;

using System.Collections.Immutable;

public record RecursiveBacktrackerState<T>(
    Maze<T> Maze, T CurrentVertex, ImmutableStack<T> Path, ImmutableList<T> Visited);