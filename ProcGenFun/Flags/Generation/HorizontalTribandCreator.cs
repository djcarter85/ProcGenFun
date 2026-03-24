namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class HorizontalTribandCreator
{
    public static IDistribution<Flag> Dist() =>
        from top in FlagColours.AllDist()
        from middle in FlagColours.AllowedAdjacentToDist(top)
        from bottom in FlagColours.AllowedAdjacentToDist(middle)
        from fimbriation in FimbriationDist([top, middle, bottom])
        from chargeType in ChargeTypeDist()
        from charge in FlagChargeCreator.ChargesDist(chargeType, backgroundColours: [middle], size: FlagChargeSize.Small, horizontalLocation: FlagChargeHorizontalLocation.Centre, verticalLocation: FlagChargeVerticalLocation.Centre)
        select new Flag(new FlagPattern.HorizontalTriband(top, middle, bottom, fimbriation), charge);

    private static IDistribution<FlagColour?> FimbriationDist(IEnumerable<FlagColour> adjacentColours) =>
        from shouldFimbriate in Bernoulli.FromRatio(1, 5)
        from fimbriation in FimbriationDist(shouldFimbriate, adjacentColours)
        select fimbriation;

    private static IDistribution<FlagColour?> FimbriationDist(
        bool shouldFimbriate, IEnumerable<FlagColour> adjacentColours) =>
        shouldFimbriate
            ? FlagColours.AllowedAdjacentToDist(adjacentColours).Nullable()
            : Singleton.New<FlagColour?>(null);

    private static IDistribution<FlagChargeShape.Type?> ChargeTypeDist() =>
        WeightedDiscreteDistributionBuilder<FlagChargeShape.Type?>.Empty()
            .Add(null, 3)
            .Add(FlagChargeShape.Type.StarBand, 1)
            .Build();
}