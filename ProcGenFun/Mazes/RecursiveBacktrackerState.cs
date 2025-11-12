namespace ProcGenFun.Mazes;

using System.Collections.Immutable;

public record RecursiveBacktrackerState(Maze Maze, Cell CurrentCell, ImmutableStack<Cell> Path, ImmutableList<Cell> Visited);