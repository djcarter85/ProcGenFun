namespace ProcGenFun.Mazes;

public record HighlightedMaze(Maze Maze, IReadOnlyList<Cell> HighlightedCells);
