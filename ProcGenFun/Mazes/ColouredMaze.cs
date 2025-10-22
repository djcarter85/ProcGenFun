namespace ProcGenFun.Mazes;

using System.Drawing;

public record ColouredMaze(Maze Maze, Func<Cell, Color> GetCellColour);
