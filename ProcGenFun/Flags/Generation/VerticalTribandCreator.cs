namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class VerticalTribandCreator
{
    public static IDistribution<Flag> Dist() =>
        from left in FlagColours.AllDist()
        from middle in FlagColours.AllowedAdjacentToDist(left)
        from leftAndRightAreSame in Bernoulli.FromRatio(2, 5)
        from right in RightColourDist(leftAndRightAreSame, left, middle)
        from chargeType in ChargeTypeDist()
        from charge in FlagChargeCreator.ChargesDist(chargeType, backgroundColours: [middle], size: FlagChargeSize.Medium, FlagChargeLocation.Centre)
        select new Flag(new FlagPattern.VerticalTriband(left, middle, right), charge);

    private static IDistribution<FlagColour> RightColourDist(
        bool leftAndRightAreSame, FlagColour left, FlagColour middle) =>
        leftAndRightAreSame
            ? Singleton.New(left)
            : FlagColours.AllowedAdjacentToExceptingDist(adjacentColour: middle, exceptColour: left);

    private static IDistribution<FlagChargeShape.Type?> ChargeTypeDist() =>
        WeightedDiscreteDistributionBuilder<FlagChargeShape.Type?>.Empty()
            .Add(null, 3)
            .Add(FlagChargeShape.Type.Star, 1)
            .Build();
}