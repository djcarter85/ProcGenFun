namespace ProcGenFun.Mazes;

using System.Drawing;

public record HighlightedMaze(Maze Maze, Func<Cell, Color> GetCellColor);
