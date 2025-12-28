namespace ProcGenFun.Mazes;

public class HexGrid
{
    public HexGrid(int maxDistanceFromOrigin)
    {
        this.MaxDistanceFromOrigin = maxDistanceFromOrigin;

        var cells =
            from q in RangeBetweenInclusive(-maxDistanceFromOrigin, maxDistanceFromOrigin)
            from r in RangeBetweenInclusive(-maxDistanceFromOrigin, maxDistanceFromOrigin)
            from s in RangeBetweenInclusive(-maxDistanceFromOrigin, maxDistanceFromOrigin)
            where HexCell.CoordinatesValid(q, r, s)
            select new HexCell(q, r, s);

        this.Cells = cells.ToList();
    }

    public int MaxDistanceFromOrigin { get; }

    public IReadOnlyList<HexCell> Cells { get; }

    public IEnumerable<HexCell> GetNeighbours(HexCell cell) =>
        from direction in HexDirection.GetAll()
        let neighbour = cell.GetNeighbourInDirection(direction)
        where this.IsWithinBounds(neighbour)
        select neighbour;

    private static IEnumerable<int> RangeBetweenInclusive(int lowerBound, int upperBound) =>
        Enumerable.Range(lowerBound, upperBound - lowerBound + 1);

    public bool IsWithinBounds(HexCell cell) => cell.DistanceFromOrigin <= this.MaxDistanceFromOrigin;
}