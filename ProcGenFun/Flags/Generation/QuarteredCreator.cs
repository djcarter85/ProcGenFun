namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Flags.Model;
using RandN;
using RandN.Extensions;

public static class QuarteredCreator
{
    public static IDistribution<Flag> Dist() =>
        from topLeft in FlagColours.AllDist()
        from topRight in FlagColours.AllowedAdjacentToDist(topLeft)
        from bottomRight in FlagColours.AllowedAdjacentToDist(topRight)
        from bottomLeft in FlagColours.AllowedAdjacentToDist([topLeft, bottomRight])
        select new Flag(new FlagPattern.Quartered(topLeft, topRight, bottomRight, bottomLeft), []);
}