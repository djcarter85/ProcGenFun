namespace ProcGenFun.Mazes;

using System.Drawing;

public record ColouredMaze(Maze<RectCell> Maze, Func<RectCell, Color> GetCellColour);
