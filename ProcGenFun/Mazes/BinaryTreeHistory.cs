namespace ProcGenFun.Mazes;

using System.Collections.Immutable;

public record BinaryTreeHistory(Maze Initial, ImmutableList<BinaryTreeStep> Steps, Maze Final);
