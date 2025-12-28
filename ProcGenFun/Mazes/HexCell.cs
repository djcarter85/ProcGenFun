namespace ProcGenFun.Mazes;

public record HexCell
{
    public HexCell(int q, int r, int s)
    {
        if (!CoordinatesValid(q, r, s))
        {
            throw new ArgumentException("q, r and s must sum to zero");
        }

        this.Q = q;
        this.R = r;
        this.S = s;
    }

    public int Q { get; }

    public int R { get; }

    public int S { get; }

    public int DistanceFromOrigin => Coordinates.Select(Math.Abs).Max();

    private IEnumerable<int> Coordinates => [this.Q, this.R, this.S];

    public static bool CoordinatesValid(int q, int r, int s) => q + r + s == 0;

    public HexCell GetNeighbourInDirection(HexDirection direction) =>
        new(Q + direction.Q, R + direction.R, S + direction.S);
}