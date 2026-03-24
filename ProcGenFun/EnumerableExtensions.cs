namespace ProcGenFun;

public static class EnumerableExtensions
{
    public static IReadOnlyList<T> ToReadOnly<T>(this IEnumerable<T> source) => source.ToList();

    public static IEnumerable<T> ExcludingNull<T>(this IEnumerable<T?> source) where T : struct => source.OfType<T>();
}