namespace ProcGenFun.Mazes;

using System.Collections.Immutable;

public record SidewinderStep(Maze<RectCell> Maze, ImmutableList<RectCell> RunBeforeWallRemoved, ImmutableList<RectCell> Run);