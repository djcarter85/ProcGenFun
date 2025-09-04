namespace ProcGenFun.Distributions;

using RandN;
using System.Diagnostics.CodeAnalysis;
using RandN.Extensions;
using RandN.Distributions;

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

    public static IDistribution<IEnumerable<T>> Repeat<T>(this IDistribution<T> dist, int count)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be non-negative");
        }

        if (count == 0)
        {
            return Singleton.New(Enumerable.Empty<T>());
        }

        return
            from values in dist.Repeat(count - 1)
            from value in dist
            select values.Append(value);
    }
}
