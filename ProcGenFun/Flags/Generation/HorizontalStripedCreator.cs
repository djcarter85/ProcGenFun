namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Extensions;

public static class HorizontalStripedCreator
{
    public static IDistribution<Flag> Dist() =>
        from colour1 in FlagColours.AllDist()
        from colour2 in FlagColours.AllowedAdjacentToDist(colour1)
        from stripeCount in UniformDistribution.Create([5, 7, 9, 11, 13])
        select new Flag(new FlagPattern.HorizontalStriped(colour1, colour2, stripeCount), []);
}