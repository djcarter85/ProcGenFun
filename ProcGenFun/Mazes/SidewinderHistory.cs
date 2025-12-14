namespace ProcGenFun.Mazes;

using System.Collections.Immutable;

public record SidewinderHistory(Maze<Cell> Initial, ImmutableList<SidewinderStep> Steps, Maze<Cell> Current);
