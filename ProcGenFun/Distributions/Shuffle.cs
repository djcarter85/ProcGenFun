using RandN;
using RandN.Distributions;
using System.Diagnostics.CodeAnalysis;

namespace ProcGenFun.Distributions;

public static class Shuffle
{
    public static IDistribution<IReadOnlyList<T>> New<T>(IReadOnlyList<T> items) =>
        new ShuffleDistribution<T>(items);

    private class ShuffleDistribution<T>(IReadOnlyList<T> items) : IDistribution<IReadOnlyList<T>>
    {
        public IReadOnlyList<T> Sample<TRng>(TRng rng)
            where TRng : notnull, IRng
        {
            // This is the Fisher-Yates algorithm.
            var array = items.ToArray();

            for (var i = 0; i < array.Length - 2; i++)
            {
                var j = Uniform.NewInclusive(i, array.Length - 1).Sample(rng);
                SwapItems(array, i, j);
            }

            return array;
        }

        private static void SwapItems(T[] array, int i, int j)
        {
            var itemI = array[i];
            var itemJ = array[j];

            array[i] = itemJ;
            array[j] = itemI;
        }

        public bool TrySample<TRng>(TRng rng, [MaybeNullWhen(false)] out IReadOnlyList<T> result)
            where TRng : notnull, IRng
        {
            throw new NotImplementedException();
        }
    }
}
