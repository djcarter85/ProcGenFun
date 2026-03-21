namespace ProcGenFun.Maps;

using System.Diagnostics.CodeAnalysis;
using ProcGenFun.Distributions;
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

public static class Perlin
{
    public static IDistribution<IFunction1> Perlin1Dist(double min, double max, int frequency)
    {
        var step = (max - min) / frequency;
        var xValues = Enumerable.Range(0, frequency + 1).Select(i => min + i * step);
        
        var amplitudeDist = Uniform.NewInclusive(-1d, 1d);

        return
            from integerAmplitudes in xValues.ToDictionaryDist(amplitudeDist)
            select (IFunction1)new Perlin1(integerAmplitudes, step, min);
    }
    
    public static IDistribution<IFunction2> Perlin2Dist(float min, float max, int frequency)
    {
        var step = (max - min) / frequency;
        var values = Enumerable.Range(0, frequency + 1).Select(i => min + i * step).ToList();
        var points =
            from x in values
            from y in values
            select new Point2(x, y);

        return
            from gradientVectors in points.ToDictionaryDist(GradientVectorDist())
            select (IFunction2)new Perlin2(gradientVectors, step, min);
    }

    private static IDistribution<Vector2> GradientVectorDist() =>
        from thetaRadians in Uniform.New(0f, 2 * MathF.PI)
        select Vector2.Unit(thetaRadians);

    private class Perlin1(IReadOnlyDictionary<double, double> integerAmplitudes, double step, double min) : IFunction1
    {
        public double Evaluate(double x)
        {
            var xBefore = (int)((x - min) / step) + min;
            var xAfter = xBefore + step;

            var t = (x - xBefore) / step;
            
            var yBefore = integerAmplitudes[xBefore];
            var yAfter = integerAmplitudes[xAfter];
            
            return yBefore + t * (yAfter - yBefore);
        }
    }
    
    private class Perlin2(IReadOnlyDictionary<Point2, Vector2> gradientVectors, float step, float min) : IFunction2
    {
        public float Evaluate(Point2 point)
        {
            // TODO: add argument validation
            
            var floorPointX = (int)((point.X - min) / step) + min;
            var floorPointY = (int)((point.Y - min) / step) + min;
            
            var floorPoint = new Point2(floorPointX, floorPointY);

            IEnumerable<Point2> cornerPoints =
            [
                floorPoint,
                floorPoint + Vector2.FromXY(step, 0),
                floorPoint + Vector2.FromXY(0, step),
                floorPoint + Vector2.FromXY(step, step)
            ];

            return cornerPoints.Sum(cp => Dot(gradientVectors[cp], point - cp));
        }

        private static float Dot(Vector2 v1, Vector2 v2) => v1.X * v2.X + v1.Y * v2.Y;
    }
}

public record Point2(float X, float Y)
{
    public static Vector2 operator -(Point2 left, Point2 right) => 
        Vector2.FromXY(left.X - right.X, left.Y - right.Y);
    
    public static Point2 operator +(Point2 left, Vector2 right) => 
        new(left.X + right.X, left.Y + right.Y);
}

public interface IFunction2
{
    float Evaluate(Point2 point);
}

public interface IFunction1
{
    double Evaluate(double x);
}