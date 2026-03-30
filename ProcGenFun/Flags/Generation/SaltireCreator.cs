namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class SaltireCreator
{
    public static IDistribution<Flag> Dist() =>
        from northSouthField in FlagColours.AllDist()
        from fieldColoursAreSame in Bernoulli.FromRatio(4, 5)
        from eastWestfield in EastWestFieldDist(northSouthField, fieldColoursAreSame)
        from foreground in FlagColours.AllowedAdjacentToDist([northSouthField, eastWestfield])
        from fimbriation in FimbriationDist(fieldColoursAreSame, northSouthField, eastWestfield, foreground)
        select new Flag(new FlagPattern.Saltire(northSouthField, eastWestfield, foreground, fimbriation), []);

    private static IDistribution<FlagColour?> FimbriationDist(
        bool fieldColoursAreSame, FlagColour northSouthField, FlagColour eastWestfield, FlagColour foreground) =>
        !fieldColoursAreSame
            ? Singleton.New<FlagColour?>(null)
            : Bernoulli.FromRatio(1, 10)
                .SelectMany(shouldFimbriate =>
                    shouldFimbriate
                        ? FlagColours.AllowedAdjacentToDist([northSouthField, eastWestfield, foreground]).Nullable()
                        : Singleton.New<FlagColour?>(null));

    private static IDistribution<FlagColour> EastWestFieldDist(
        FlagColour northSouthField, bool fieldColoursAreSame) =>
        fieldColoursAreSame ? Singleton.New(northSouthField) : FlagColours.AllExceptDist(northSouthField);
}