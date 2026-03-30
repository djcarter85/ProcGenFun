namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Flags.Model;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class QuarteredCreator
{
    public static IDistribution<Flag> Dist() =>
        from topLeft in FlagColours.AllDist()
        from topRight in FlagColours.AllowedAdjacentToDist(topLeft)
        from bottomRight in BottomRightDist(topLeft, topRight)
        from bottomLeft in BottomLeftDist(topLeft, topRight, bottomRight)
        select new Flag(new FlagPattern.Quartered(topLeft, topRight, bottomRight, bottomLeft), []);

    private static IDistribution<FlagColour> BottomRightDist(FlagColour topLeft, FlagColour topRight) =>
        Bernoulli.FromRatio(1, 2)
            .SelectMany(shouldBeSameAsTopLeft =>
                shouldBeSameAsTopLeft
                    ? Singleton.New(topLeft)
                    : FlagColours.AllowedAdjacentToDist(topRight));

    private static IDistribution<FlagColour> BottomLeftDist(
        FlagColour topLeft, FlagColour topRight, FlagColour bottomRight) =>
        Bernoulli.FromRatio(1, 2)
            .SelectMany(shouldBeSameAsTopRight =>
                shouldBeSameAsTopRight
                    ? Singleton.New(topRight)
                    : FlagColours.AllowedAdjacentToDist([topLeft, bottomRight]));
}