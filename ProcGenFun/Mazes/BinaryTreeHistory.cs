namespace ProcGenFun.Mazes;

using System.Collections.Immutable;

public record BinaryTreeHistory(Maze<RectCell> Initial, ImmutableList<BinaryTreeStep> Steps, Maze<RectCell> Final);
