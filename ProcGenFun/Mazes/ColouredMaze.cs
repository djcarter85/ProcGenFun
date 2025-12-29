namespace ProcGenFun.Mazes;

using System.Drawing;

public record ColouredMaze<TCell>(Maze<TCell> Maze, Func<TCell, Color> GetCellColour)
    where TCell : notnull;
