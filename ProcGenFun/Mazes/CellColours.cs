namespace ProcGenFun.Mazes;

using System.Drawing;
using System.Linq;

public static class CellColours
{
    public static Func<TCell, Color> Base<TCell>() => _ => Theme.White;

    public static Func<TCell, Color> Highlighted<TCell>(IEnumerable<TCell> highlightedCells) =>
        c => highlightedCells.Contains(c) ? Theme.Blue300 : Theme.White;

    public static Func<TCell, Color> RB<TCell>(TCell currentCell, IEnumerable<TCell> visitedCells, IEnumerable<TCell> path) =>
        c => Equals(c, currentCell) ? Theme.Blue300 :
            path.Contains(c) ? Theme.Amber100 :
            visitedCells.Contains(c) ? Theme.White :
            RBUnvisited<TCell>()(c);

    public static Func<TCell, Color> RBUnvisited<TCell>() => _ => Theme.Grey200;
}