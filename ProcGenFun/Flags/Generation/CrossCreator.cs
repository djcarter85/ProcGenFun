namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Extensions;

public static class CrossCreator
{
    public static IDistribution<Flag> Dist() =>
        from background in FlagColours.AllDist()
        from foreground in FlagColours.AllowedAdjacentToDist(background)
        from crossType in UniformDistribution.Create([CrossType.Regular, CrossType.Nordic])
        select new Flag(new FlagPattern.Cross(background, foreground, crossType), []);
}