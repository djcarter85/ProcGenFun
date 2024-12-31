namespace ProcGenFun.Distributions;

using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class UniformDistribution
{
    public static IDistribution<T> CreateOrThrow<T>(IEnumerable<T> items)
    {
        if (TryCreate(items, out var dist))
        {
            return dist;
        }

        throw new InvalidOperationException(
            "There are no items from which to create a uniform distribution.");
    }

    public static bool TryCreate<T>(IEnumerable<T> items, out IDistribution<T> dist)
    {
        var list = items.ToList();

        if (list.Count == 0)
        {
            dist = default!;
            return false;
        }
        else
        {
            dist =
                from index in Uniform.NewInclusive(0, list.Count - 1)
                select list[index];
            return true;
        }
    }
}
