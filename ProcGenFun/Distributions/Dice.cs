namespace ProcGenFun.Distributions;

using RandN;
using RandN.Distributions;

public static class Dice
{
    public static IDistribution<int> D6Dist() => Uniform.NewInclusive(1, 6);
}
