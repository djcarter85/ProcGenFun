namespace ProcGenFun.Perlin;

using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class Perlin
{
    public static IDistribution<IFunction1> Perlin1Dist(double min, double max, int frequency, double amplitude)
    {
        var step = (max - min) / frequency;
        var xValues = Enumerable.Range(0, frequency + 1).Select(i => min + i * step);

        var amplitudeDist = Uniform.NewInclusive(-amplitude, amplitude);

        return
            from integerAmplitudes in xValues.ToDictionaryDist(amplitudeDist)
            select (IFunction1)new Perlin1(integerAmplitudes, step, min);
    }

    private class Perlin1(IReadOnlyDictionary<double, double> integerAmplitudes, double step, double min) : IFunction1
    {
        public double Evaluate(double x)
        {
            if (integerAmplitudes.TryGetValue(x, out var value))
            {
                return value;
            }

            var xBefore = step * (int)((x - min) / step) + min;
            var xAfter = xBefore + step;

            var t = (x - xBefore) / step;

            var yBefore = integerAmplitudes[xBefore];
            var yAfter = integerAmplitudes[xAfter];

            return yBefore + t * (yAfter - yBefore);
        }
    }
}