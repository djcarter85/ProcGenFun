namespace ProcGenFun.Mazes;

using System.Collections.Immutable;

public record SidewinderHistory(Maze Initial, ImmutableList<SidewinderStep> Steps, Maze Current);
