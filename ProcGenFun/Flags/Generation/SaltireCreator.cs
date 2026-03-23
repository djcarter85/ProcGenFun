namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Flags.Model;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class SaltireCreator
{
    public static IDistribution<Flag> Dist()
    {
        IDistribution<FlagColour> EastWestFieldDist(FlagColour northSouthField, bool fieldColoursAreSame) =>
            fieldColoursAreSame ? Singleton.New(northSouthField) : FlagColours.AllExceptDist(northSouthField);

        return from northSouthField in FlagColours.AllDist()
            from fieldColoursAreSame in Bernoulli.FromRatio(4, 5)
            from eastWestfield in EastWestFieldDist(northSouthField, fieldColoursAreSame)
            from foreground in FlagColours.AllowedAdjacentToDist([northSouthField, eastWestfield])
            select new Flag(new FlagPattern.Saltire(northSouthField, eastWestfield, foreground), []);
    }
}