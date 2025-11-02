namespace ProcGenFun.Mazes;

using System.Drawing;
using System.Linq;

public static class CellColours
{
    public static Func<Cell, Color> Base() => _ => Theme.White;

    public static Func<Cell, Color> Highlighted(IEnumerable<Cell> highlightedCells) =>
        c => highlightedCells.Contains(c) ? Theme.Blue300 : Theme.White;

    public static Func<Cell, Color> RB(Cell currentCell, IEnumerable<Cell> visitedCells, IEnumerable<Cell> path) =>
        c => c == currentCell ? Theme.Blue300 :
            path.Contains(c) ? Theme.Amber100 :
            visitedCells.Contains(c) ? Theme.White :
            RBUnvisited()(c);

    public static Func<Cell, Color> RBUnvisited() => _ => Theme.Grey200;
}