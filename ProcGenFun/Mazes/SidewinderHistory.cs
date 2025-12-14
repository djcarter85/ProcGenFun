namespace ProcGenFun.Mazes;

using System.Collections.Immutable;

public record SidewinderHistory(Maze<RectCell> Initial, ImmutableList<SidewinderStep> Steps, Maze<RectCell> Current);
