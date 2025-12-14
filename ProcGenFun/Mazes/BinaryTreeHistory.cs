namespace ProcGenFun.Mazes;

using System.Collections.Immutable;

public record BinaryTreeHistory(Maze<Cell> Initial, ImmutableList<BinaryTreeStep> Steps, Maze<Cell> Final);
