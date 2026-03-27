namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Flags.Model;
using RandN;
using RandN.Extensions;

public static class PartyPerPallCreator
{
    public static IDistribution<Flag> Dist() =>
        from left in FlagColours.AllDist()
        from top in FlagColours.AllowedAdjacentToDist(left)
        from bottom in FlagColours.AllowedAdjacentToDist([left, top])
        select new Flag(new FlagPattern.PartyPerPall(left, top, bottom), []);
}