namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Extensions;

public static class SolidFlagCreator
{
    public static IDistribution<Flag> Dist() =>
        from colour in FlagColours.AllDist()
        from chargeType in ChargeTypeDist()
        from charges in FlagChargeCreator.ChargesDist(chargeType, backgroundColours: [colour], size: FlagChargeSize.Large, horizontalLocation: FlagChargeHorizontalLocation.Centre, verticalLocation: FlagChargeVerticalLocation.Centre)
        select new Flag(new FlagPattern.Solid(colour), charges);

    private static IDistribution<FlagChargeShape.Type?> ChargeTypeDist() =>
        WeightedDiscreteDistributionBuilder<FlagChargeShape.Type?>.Empty()
            .Add(null, 2)
            .Add(FlagChargeShape.Type.Star, 4)
            .Add(FlagChargeShape.Type.Circle, 4)
            .Add(FlagChargeShape.Type.Plus, 1)
            .Build();
}