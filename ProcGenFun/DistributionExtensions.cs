namespace ProcGenFun;

using RandN;
using System.Diagnostics.CodeAnalysis;

public static class DistributionExtensions
{
    public static IDistribution<IEnumerable<T>> Repeat<T>(this IDistribution<T> dist, int count) =>
        new RepeatDistribution<T>(dist, count);

    private class RepeatDistribution<T>(IDistribution<T> dist, int count) : IDistribution<IEnumerable<T>>
    {
        public IEnumerable<T> Sample<TRng>(TRng rng) where TRng : notnull, IRng
        {
            for (int i = 0; i < count; i++)
            {
                yield return dist.Sample(rng);
            }
        }

        public bool TrySample<TRng>(TRng rng, [MaybeNullWhen(false)] out IEnumerable<T> result) where TRng : notnull, IRng
        {
            // Assume we're working with distributions which always generate a valid value.
            result = Sample(rng);
            return true;
        }
    }
}
