namespace ProcGenFun.Distributions;

using ProcGenFun.Mazes;
using RandN;
using System.Diagnostics.CodeAnalysis;

public static class DistributionExtensions
{
    public static IDistribution<IEnumerable<T>> Repeat<T>(this IDistribution<T> dist, int count) =>
        new RepeatDistribution<T>(dist, count);

    public static IDistribution<IEnumerable<T>> Iterate<T>(
        this IDistribution<T> initialDist,
        Func<T, IDistribution<bool>> shouldStopDist,
        Func<T, IDistribution<T>> nextStepDist) =>
        new IterateDistribution<T>(initialDist, shouldStopDist, nextStepDist);

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

    private class IterateDistribution<T> : IDistribution<IEnumerable<T>>
    {
        private readonly IDistribution<T> initialDist;
        private readonly Func<T, IDistribution<bool>> shouldStopDist;
        private readonly Func<T, IDistribution<T>> nextStepDist;

        public IterateDistribution(
            IDistribution<T> initialDist,
            Func<T, IDistribution<bool>> shouldStopDist,
            Func<T, IDistribution<T>> nextStepDist)
        {
            this.initialDist = initialDist;
            this.shouldStopDist = shouldStopDist;
            this.nextStepDist = nextStepDist;
        }

        public IEnumerable<T> Sample<TRng>(TRng rng) where TRng : notnull, IRng
        {
            var initial = this.initialDist.Sample(rng);

            var history = new History<T>([], Current: initial);

            while (!this.shouldStopDist(history.Current).Sample(rng))
            {
                var newCurrent = this.nextStepDist(history.Current).Sample(rng);

                history = new History<T>(history.Previous.Add(history.Current), newCurrent);
            }

            return history.Previous.Append(history.Current);
        }

        public bool TrySample<TRng>(TRng rng, [MaybeNullWhen(false)] out IEnumerable<T> result) where TRng : notnull, IRng
        {
            result = Sample(rng);
            return true;
        }
    }
}
