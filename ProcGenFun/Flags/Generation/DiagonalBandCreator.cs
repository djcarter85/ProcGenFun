namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class DiagonalBandCreator
{
    public static IDistribution<Flag> Dist() =>
        from field in FlagColours.AllDist()
        from band in FlagColours.AllowedAdjacentToDist(field)
        from diagonal in UniformDistribution.Create([Diagonal.Down, Diagonal.Up])
        from fimbriation in FimbriationDist(field, band)
        select new Flag(new FlagPattern.DiagonalBand(field, band, diagonal, fimbriation), []);

    private static IDistribution<FlagColour?> FimbriationDist(FlagColour field, FlagColour band) =>
        Bernoulli.FromRatio(3, 10)
            .SelectMany(shouldFimbriate =>
                shouldFimbriate
                    ? FlagColours.AllowedAdjacentToDist([field, band]).Nullable()
                    : Singleton.New<FlagColour?>(null));
}