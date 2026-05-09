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
        from charges in ChargesDist(cantonColour)
        select new Flag(new FlagPattern.Canton(field, cantonColour), charges);

    private static IDistribution<IReadOnlyList<FlagCharge>> ChargesDist(FlagColour cantonColour) =>
        WeightedDiscreteDistributionBuilder<IDistribution<IReadOnlyList<FlagCharge>>>.Empty()
            .Add(FlagChargeCreator.NoChargesDist(), 1)
            .Add(FlagChargeCreator.StarChargeDist([cantonColour], FlagChargeSize.Medium,  FlagChargeLocation.TopHalfLeftHalf), 8)
            .Add(FlagChargeCreator.PlusChargeDist([cantonColour], FlagChargeSize.Medium,  FlagChargeLocation.TopHalfLeftHalf), 4)
            .Add(FlagChargeCreator.CircleChargeDist([cantonColour], FlagChargeSize.Medium,  FlagChargeLocation.TopHalfLeftHalf), 2)
            .Build()
            .Flatten();
}