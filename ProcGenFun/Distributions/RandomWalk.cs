namespace ProcGenFun.Distributions;

using System.Diagnostics.CodeAnalysis;
using RandN;

public static class RandomWalk
{
    public static IDistribution<IEnumerable<T>> New<T>(T initial, Func<T, IDistribution<T>> stepDist) =>
        new RandomWalkDistribution<T>(initial, stepDist);

    private class RandomWalkDistribution<T>(T initial, Func<T, IDistribution<T>> stepDist) 
        : IDistribution<IEnumerable<T>>
    {
        public IEnumerable<T> Sample<TRng>(TRng rng)
            where TRng : notnull, IRng
        {
            var state = initial;

            while (true)
            {
                yield return state;
                state = stepDist(state).Sample(rng);
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