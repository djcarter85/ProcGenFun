namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Extensions;

public static class CantonFlagCreator
{
    public static IDistribution<Flag> Dist() =>
        from field in FlagColours.AllDist()
        from cantonColour in FlagColours.AllowedAdjacentToDist(field)
        from chargeType in ChargeTypeDist()
        from charges in FlagChargeCreator.ChargesDist(chargeType, [cantonColour], FlagChargeSize.Medium,  FlagChargeLocation.TopLeft)
        select new Flag(new FlagPattern.Canton(field, cantonColour), charges);

    private static IDistribution<FlagChargeShape.Type?> ChargeTypeDist() =>
        WeightedDiscreteDistributionBuilder<FlagChargeShape.Type?>.Empty()
            .Add(null, 1)
            .Add(FlagChargeShape.Type.Star, 8)
            .Add(FlagChargeShape.Type.Plus, 4)
            .Add(FlagChargeShape.Type.Circle, 2)
            .Build();
}