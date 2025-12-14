namespace ProcGenFun.Mazes;

using System.Drawing;

public record ColouredMaze(Maze<Cell> Maze, Func<Cell, Color> GetCellColour);
