namespace ProcGenFun;

public static class EnumerableExtensions
{
    public static IReadOnlyList<T> ToReadOnly<T>(this IEnumerable<T> source) => source.ToList();
}