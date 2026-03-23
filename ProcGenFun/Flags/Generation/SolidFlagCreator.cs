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
        from charges in FlagChargeCreator.ChargesDist(chargeType, backgroundColour: colour, size: 3, horizontalLocation: FlagChargeHorizontalLocation.Centre)
        select new Flag(new FlagPattern.Solid(colour), charges);

    private static IDistribution<FlagChargeShape.Type?> ChargeTypeDist() =>
        WeightedDiscreteDistributionBuilder<FlagChargeShape.Type?>.Empty()
            .Add(null, 1)
            .Add(FlagChargeShape.Type.Star, 2)
            .Add(FlagChargeShape.Type.Circle, 2)
            .Build();
}