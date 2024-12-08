namespace ProcGenFun;

using RandN;
using RandN.Distributions;
using System.Diagnostics.CodeAnalysis;

public static class Normal
{
    public static IDistribution<double> New() => new NormalDistribution();

    private class NormalDistribution : IDistribution<double>
    {
        public double Sample<TRng>(TRng rng) where TRng : notnull, IRng
        {
            // This class uses the Box-Muller algorithm.
            var uniformDist = Uniform.New(0d, 1d);
            
            var u1 = uniformDist.Sample(rng);
            var u2 = uniformDist.Sample(rng);

            return 
                Math.Sqrt(-2.0 * Math.Log(u1))
                * Math.Cos(2.0 * Math.PI * u2);
        }

        public bool TrySample<TRng>(TRng rng, [MaybeNullWhen(false)] out double result) where TRng : notnull, IRng
        {
            result = Sample(rng);
            return true;
        }
    }
}
