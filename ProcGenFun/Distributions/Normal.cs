namespace ProcGenFun.Distributions;

using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class Normal
{
    public static IDistribution<double> New() =>
        // This calculation uses the Box-Muller algorithm.
        from u1 in Uniform.New(0d, 1d)
        from u2 in Uniform.New(0d, 1d)
        select
            Math.Sqrt(-2.0 * Math.Log(u1))
            * Math.Cos(2.0 * Math.PI * u2);
}
