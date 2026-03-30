namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Flags.Model;
using RandN;
using RandN.Extensions;

public static class QuarteredCreator
{
    public static IDistribution<Flag> Dist() =>
        from topLeft in FlagColours.AllDist()
        from topRight in FlagColours.AllowedAdjacentToDist(topLeft)
        from bottomRight in BottomRightDist(topRight)
        from bottomLeft in BottomLeftDist(topLeft, bottomRight)
        select new Flag(new FlagPattern.Quartered(topLeft, topRight, bottomRight, bottomLeft), []);

    private static IDistribution<FlagColour> BottomRightDist(FlagColour topRight) => 
        FlagColours.AllowedAdjacentToDist(topRight);

    private static IDistribution<FlagColour> BottomLeftDist(FlagColour topLeft, FlagColour bottomRight) => 
        FlagColours.AllowedAdjacentToDist([topLeft, bottomRight]);
}