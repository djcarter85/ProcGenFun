namespace ProcGenFun.Distributions;

using RandN;
using System.Diagnostics.CodeAnalysis;
using RandN.Extensions;

public static class DistributionExtensions
{
    public static IDistribution<double> EstimateProbability<T>(
        this IDistribution<T> dist,
        Func<T, bool> predicate,
        int sampleCount) =>
        from samples in dist.Repeat(sampleCount)
        select (double)samples.Count(predicate) / sampleCount;

    public static IDistribution<IReadOnlyDictionary<T, double>> EstimateProbabilityDensities<T>(
        this IDistribution<T> dist,
        int sampleCount)
        where T : notnull =>
        from samples in dist.Repeat(sampleCount)
        select GetProbabilityDensities(sampleCount, samples);

    private static IReadOnlyDictionary<T, double> GetProbabilityDensities<T>(
        int sampleCount, IEnumerable<T> samples)
        where T : notnull =>
        samples
            .GroupBy(s => s)
            .ToDictionary(g => g.Key, g => (double)g.Count() / sampleCount);

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
