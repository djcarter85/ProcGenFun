namespace ProcGenFun.Mazes;

public class RectGrid
{
    private static readonly IEnumerable<RectDirection> AllDirections =
        [RectDirection.North, RectDirection.East, RectDirection.South, RectDirection.West];

    public RectGrid(int width, int height)
    {
        this.Width = width;
        this.Height = height;
        this.Cells =
            (from y in this.RowIndices
             from x in this.ColumnIndices
             select new RectCell(x, y))
             .ToList();
    }

    public int Width { get; }

    public int Height { get; }

    public IReadOnlyList<RectCell> Cells { get; }

    public IEnumerable<int> ColumnIndices => Enumerable.Range(0, this.Width);

    public IEnumerable<int> RowIndices => Enumerable.Range(0, this.Height);

    public bool CanRemoveWall(RectCell cell, RectDirection direction) =>
        this.AdjacentCellOrNull(cell, direction) != null;

    public bool AdjacentCellExists(RectCell cell, RectDirection direction) =>
        IsValid(AdjacentCell(cell, direction));

    public RectCell? AdjacentCellOrNull(RectCell cell, RectDirection direction)
    {
        var potentialAdjacentCell = AdjacentCell(cell, direction);
        return IsValid(potentialAdjacentCell) ? potentialAdjacentCell : null;
    }

    public IEnumerable<RectCell> GetNeighbours(RectCell cell) =>
        from direction in AllDirections
        let adjacentCellOrNull = AdjacentCellOrNull(cell, direction)
        where adjacentCellOrNull != null
        select adjacentCellOrNull;

    private static RectCell AdjacentCell(RectCell cell, RectDirection direction) =>
        direction switch
        {
            RectDirection.North => cell with { Y = cell.Y - 1 },
            RectDirection.East => cell with { X = cell.X + 1 },
            RectDirection.South => cell with { Y = cell.Y + 1 },
            RectDirection.West => cell with { X = cell.X - 1 },
            _ => throw new ArgumentOutOfRangeException(nameof(direction)),
        };

    private bool IsValid(RectCell cell) =>
        cell.X >= 0 && cell.X < this.Width && cell.Y >= 0 && cell.Y < this.Height;
}