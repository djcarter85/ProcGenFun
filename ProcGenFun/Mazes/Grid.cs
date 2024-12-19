namespace ProcGenFun.Mazes;

public class Grid
{
    public Grid(int width, int height)
    {
        this.Width = width;
        this.Height = height;
        this.Cells =
            (from y in Enumerable.Range(0, this.Height)
             from x in Enumerable.Range(0, this.Width)
             select new Cell(x, y))
             .ToList();
    }

    public int Width { get; }

    public int Height { get; }

    public IEnumerable<Cell> Cells { get; }

    public bool CanRemoveWall(Cell cell, Direction direction) =>
        this.AdjacentCellOrNull(cell, direction) != null;

    public Cell? AdjacentCellOrNull(Cell cell, Direction direction)
    {
        var potentialAdjacentCell = AdjacentCell(cell, direction);
        return IsValid(potentialAdjacentCell) ? potentialAdjacentCell : null;
    }

    private static Cell AdjacentCell(Cell cell, Direction direction) =>
        direction switch
        {
            Direction.North => cell with { Y = cell.Y - 1 },
            Direction.East => cell with { X = cell.X + 1 },
            Direction.South => cell with { Y = cell.Y + 1 },
            Direction.West => cell with { X = cell.X - 1 },
            _ => throw new ArgumentOutOfRangeException(nameof(direction)),
        };

    private bool IsValid(Cell cell) =>
        cell.X >= 0 && cell.X < this.Width && cell.Y >= 0 && cell.Y < this.Height;
}
