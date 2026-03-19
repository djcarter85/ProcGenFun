namespace ProcGenFun.Maps;

using System.Diagnostics.CodeAnalysis;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class MapCreator
{
    public static IDistribution<Map> MapDist()
    {
        const int gridSquares = 20;

        var gridPoints =
            (from x in Enumerable.Range(0, gridSquares + 1)
             from y in Enumerable.Range(0, gridSquares + 1)
             select new GridPoint(x, y)).ToList();

        return 
            from gridPointVectors in GradientPointVectorsDist(gridPoints)
            select new Map(gridPointVectors);
    }

    private static IDistribution<IReadOnlyDictionary<GridPoint, Vector2>> GradientPointVectorsDist(
        IReadOnlyList<GridPoint> gridPoints) =>
        new DictionaryDist<GridPoint, Vector2>(gridPoints, VectorDist());

    private static IDistribution<Vector2> VectorDist() =>
        from thetaRadians in Uniform.New(0f, 2 * MathF.PI)
        select Vector2.Unit(thetaRadians);

    private class DictionaryDist<TKey, TValue>(IEnumerable<TKey> gridPoints, IDistribution<TValue> valueDist)
        : IDistribution<IReadOnlyDictionary<TKey, TValue>> 
        where TKey : notnull
    {
        public IReadOnlyDictionary<TKey, TValue> Sample<TRng>(TRng rng)
            where TRng : notnull, IRng =>
            gridPoints.ToDictionary(
                k => k,
                _ => valueDist.Sample(rng));

        public bool TrySample<TRng>(
            TRng rng, [MaybeNullWhen(false)] out IReadOnlyDictionary<TKey, TValue> result)
            where TRng : notnull, IRng =>
            throw new NotImplementedException();
    }
}