namespace ProcGenFun.Mazes;

public record Cell(int X, int Y)
{
    public static IComparer<Cell> Comparer { get; } = new CellComparer();

    private class CellComparer : IComparer<Cell>
    {
        public int Compare(Cell? left, Cell? right)
        {
            if (ReferenceEquals(left, right))
            {
                return 0;
            }

            if (right is null)
            {
                return 1;
            }

            if (left is null)
            {
                return -1;
            }

            var yComparison = left.Y.CompareTo(right.Y);
            if (yComparison != 0)
            {
                return yComparison;
            }

            return left.X.CompareTo(right.X);
        }
    }
}
