namespace ProcGenFun;

using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class UniformDistribution
{
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
