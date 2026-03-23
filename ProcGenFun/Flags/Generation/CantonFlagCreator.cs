namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Flags.Model;
using RandN;
using RandN.Extensions;

public static class CantonFlagCreator
{
    public static IDistribution<Flag> Dist() =>
        from field in FlagColours.AllDist()
        from cantonColour in FlagColours.AllowedAdjacentToDist(field)
        select new Flag(new FlagPattern.Canton(field, cantonColour), []);
}