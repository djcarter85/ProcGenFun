namespace ProcGenFun.Mazes;

using System.Drawing;
using System.Linq;

public static class CellColours
{
    public static Func<RectCell, Color> Base() => _ => Theme.White;

    public static Func<RectCell, Color> Highlighted(IEnumerable<RectCell> highlightedCells) =>
        c => highlightedCells.Contains(c) ? Theme.Blue300 : Theme.White;

    public static Func<RectCell, Color> RB(RectCell currentCell, IEnumerable<RectCell> visitedCells, IEnumerable<RectCell> path) =>
        c => c == currentCell ? Theme.Blue300 :
            path.Contains(c) ? Theme.Amber100 :
            visitedCells.Contains(c) ? Theme.White :
            RBUnvisited()(c);

    public static Func<RectCell, Color> RBUnvisited() => _ => Theme.Grey200;
}