namespace ProcGenFun.Mazes;

using System.Collections.Immutable;

public record AldousBroderState(Maze Maze, Cell CurrentCell, ImmutableHashSet<Cell> Visited);
