namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Extensions;

public static class PartyPerPallCreator
{
    public static IDistribution<Flag> Dist() =>
        from left in FlagColours.AllDist()
        from top in FlagColours.AllowedAdjacentToDist(left)
        from bottom in FlagColours.AllowedAdjacentToDist([left, top])
        from chargeType in ChargeTypeDist()
        from charges in FlagChargeCreator.ChargesDist(chargeType, backgroundColours: [left], size: FlagChargeSize.Small, FlagChargeLocation.CentreFarLeft)
        select new Flag(new FlagPattern.PartyPerPall(left, top, bottom), charges);

    private static IDistribution<FlagChargeShape.Type?> ChargeTypeDist() =>
        WeightedDiscreteDistributionBuilder<FlagChargeShape.Type?>.Empty()
            .Add(null, 10)
            .Add(FlagChargeShape.Type.Star, 3)
            .Add(FlagChargeShape.Type.Circle, 1)
            .Add(FlagChargeShape.Type.Plus, 1)
            .Build();
}