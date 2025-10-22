namespace ProcGenFun.Mazes;

using System.Drawing;

public static class CellColours
{
    public static Func<Cell, Color> Base() => _ => Theme.White;

    public static Func<Cell, Color> Highlighted(IEnumerable<Cell> highlightedCells) =>
        c => highlightedCells.Contains(c) ? Theme.Blue300 : Theme.White;
}