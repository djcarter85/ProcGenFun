namespace ProcGenFun.Mazes;
using System.Collections.Immutable;

public record RBState(Maze Maze, Cell CurrentCell, ImmutableStack<Cell> Stack, ImmutableList<Cell> Visited);
