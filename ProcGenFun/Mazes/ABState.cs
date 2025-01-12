using System.Collections.Immutable;

namespace ProcGenFun.Mazes;

public record ABState(Maze Maze, Cell CurrentCell, ImmutableList<Cell> Visited);
