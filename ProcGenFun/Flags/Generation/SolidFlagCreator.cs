namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Extensions;

public static class SolidFlagCreator
{
    public static IDistribution<Flag> Dist() =>
        from colour in FlagColours.AllDist()
        from charges in ChargesDist(colour)
        select new Flag(new FlagPattern.Solid(colour), charges);

    private static IDistribution<IReadOnlyList<FlagCharge>> ChargesDist(FlagColour colour) =>
        WeightedDiscreteDistributionBuilder<IDistribution<IReadOnlyList<FlagCharge>>>.Empty()
            .Add(FlagChargeCreator.NoChargesDist(), 2)
            .Add(FlagChargeCreator.StarChargeDist(backgroundColours: [colour], FlagChargeSize.ExtraLarge, FlagChargeLocation.Centre), 4)
            .Add(FlagChargeCreator.CircleChargeDist(backgroundColours: [colour], FlagChargeSize.ExtraLarge, FlagChargeLocation.Centre), 4)
            .Add(FlagChargeCreator.PlusChargeDist(backgroundColours: [colour], FlagChargeSize.ExtraLarge, FlagChargeLocation.Centre), 1)
            .Add(FlagChargeCreator.ShieldChargeDist(backgroundColours: [colour], FlagChargeSize.ExtraLarge, FlagChargeLocation.Centre), 1)
            .Build()
            .Flatten();
}