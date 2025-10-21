namespace ProcGenFun.Distributions;

using System.Diagnostics.CodeAnalysis;
using RandN;

public static class RandomWalk
{
    public static IDistribution<IEnumerable<T>> New<T>(T initial, Func<T, IDistribution<T>> stepDist) =>
        new RandomWalkDistribution<T>(initial, stepDist);

    private class RandomWalkDistribution<T> : IDistribution<IEnumerable<T>>
    {
        private readonly T initial;
        private readonly Func<T, IDistribution<T>> stepDist;

        public RandomWalkDistribution(T initial, Func<T, IDistribution<T>> stepDist)
        {
            this.initial = initial;
            this.stepDist = stepDist;
        }

        public IEnumerable<T> Sample<TRng>(TRng rng)
            where TRng : notnull, IRng
        {
            var state = this.initial;

            while (true)
            {
                yield return state;
                state = this.stepDist(state).Sample(rng);
            }
        }

        public bool TrySample<TRng>(TRng rng, [MaybeNullWhen(false)] out IEnumerable<T> result)
            where TRng : notnull, IRng
        {
            // Assume we're working with distributions which always generate a valid value.
            result = Sample(rng);
            return true;
        }
    }
}