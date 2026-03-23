namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Extensions;

public static class HorizontalTribandCreator
{
    public static IDistribution<Flag> Dist() =>
        from top in FlagColours.AllDist()
        from middle in FlagColours.AllowedAdjacentToDist(top)
        from bottom in FlagColours.AllowedAdjacentToDist(middle)
        from chargeType in ChargeTypeDist()
        from charge in FlagChargeCreator.ChargesDist(chargeType, backgroundColours: [middle], size: FlagChargeSize.Small, horizontalLocation: FlagChargeHorizontalLocation.Centre)
        select new Flag(new FlagPattern.HorizontalTriband(top, middle, bottom), charge);

    private static IDistribution<FlagChargeShape.Type?> ChargeTypeDist() =>
        WeightedDiscreteDistributionBuilder<FlagChargeShape.Type?>.Empty()
            .Add(null, 3)
            .Add(FlagChargeShape.Type.StarBand, 1)
            .Build();
}