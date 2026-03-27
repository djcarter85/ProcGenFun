namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Flags.Model;
using RandN;
using RandN.Extensions;

public static class PallCreator
{
    public static IDistribution<Flag> Dist() =>
        from field in FlagColours.AllDist()
        from foreground in FlagColours.AllowedAdjacentToDist(field)
        select new Flag(new FlagPattern.Pall(field, foreground), []);
}