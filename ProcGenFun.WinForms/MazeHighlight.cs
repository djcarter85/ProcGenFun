namespace ProcGenFun.WinForms;
using ProcGenFun.Mazes;

public record MazeHighlight(Maze Maze, IReadOnlyList<Cell> HighlightedCells);