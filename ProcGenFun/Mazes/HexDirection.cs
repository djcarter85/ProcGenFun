namespace ProcGenFun.Mazes;

public record HexDirection
{
    private HexDirection(int q, int r, int s)
    {
        this.Q = q;
        this.R = r;
        this.S = s;
    }

    public static HexDirection NorthEast { get; } = new(1, -1, 0);

    public static HexDirection East { get; } = new(1, 0, -1);

    public static HexDirection SouthEast { get; } = new(0, 1, -1);

    public static HexDirection SouthWest { get; } = new(-1, 1, 0);

    public static HexDirection West { get; } = new(-1, 0, 1);

    public static HexDirection NorthWest { get; } = new(0, -1, 1);

    public static IEnumerable<HexDirection> GetAll() =>
    [
        NorthEast,
        East,
        SouthEast,
        SouthWest,
        West,
        NorthWest,
    ];

    public int Q { get; }
    
    public int R { get; }
    
    public int S { get; }
}