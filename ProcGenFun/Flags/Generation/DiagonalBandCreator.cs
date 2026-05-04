namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Extensions;

public static class DiagonalBandCreator
{
    public static IDistribution<Flag> Dist() =>
        from field in FlagColours.AllDist()
        from band in FlagColours.AllowedAdjacentToDist(field)
        from diagonal in UniformDistribution.Create([Diagonal.Down, Diagonal.Up])
        select new Flag(new FlagPattern.DiagonalBand(field, band, diagonal), []);
}