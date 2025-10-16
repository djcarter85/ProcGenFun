namespace ProcGenFun.Mazes;

using System.Collections.Immutable;

public record SidewinderStep(Maze Maze, ImmutableList<Cell> RunBeforeWallRemoved, ImmutableList<Cell> Run);