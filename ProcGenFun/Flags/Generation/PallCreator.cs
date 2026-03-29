namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class PallCreator
{
    public static IDistribution<Flag> Dist() =>
        from field in FlagColours.AllDist()
        from foreground in FlagColours.AllowedAdjacentToDist(field)
        from fimbriation in FimbriationDist(field, foreground)
        select new Flag(new FlagPattern.Pall(field, foreground, fimbriation), []);

    private static IDistribution<FlagColour?> FimbriationDist(FlagColour field, FlagColour foreground) =>
        from shouldFimbriate in Bernoulli.FromRatio(1, 5)
        from fimbriation in FimbriationDist(shouldFimbriate, field, foreground)
        select fimbriation;

    private static IDistribution<FlagColour?> FimbriationDist(
        bool shouldFimbriate, FlagColour field, FlagColour foreground) =>
        shouldFimbriate
            ? FlagColours.AllowedAdjacentToDist([field, foreground]).Nullable()
            : Singleton.New<FlagColour?>(null);
}