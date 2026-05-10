namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Extensions;

public static class SolidFlagCreator
{
    public static IDistribution<Flag> Dist() =>
        from colour in FlagColours.AllDist()
        from location in ChargeLocationDist()
        from charges in ChargesDist(colour, location)
        select new Flag(new FlagPattern.Solid(colour), charges);

    private static IDistribution<SolidFlagChargeLocation> ChargeLocationDist() =>
        WeightedDiscreteDistributionBuilder<SolidFlagChargeLocation>.Empty()
            .Add(SolidFlagChargeLocation.None, 1)
            .Add(SolidFlagChargeLocation.Centre, 10)
            .Add(SolidFlagChargeLocation.TopLeftCorner, 3)
            .Build();

    private static IDistribution<IReadOnlyList<FlagCharge>> ChargesDist(FlagColour colour, SolidFlagChargeLocation location) =>
        location switch {
            SolidFlagChargeLocation.TopLeftCorner => TopLeftCornerChargesDist(colour),
            SolidFlagChargeLocation.Centre => CentreChargesDist(colour),
            SolidFlagChargeLocation.None => FlagChargeCreator.NoChargesDist(),
            _ => throw new ArgumentOutOfRangeException(nameof(location), location, null)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>> CentreChargesDist(FlagColour colour) =>
        WeightedDiscreteDistributionBuilder<IDistribution<IReadOnlyList<FlagCharge>>>.Empty()
            .Add(FlagChargeCreator.StarChargeDist(backgroundColours: [colour], FlagChargeSize.ExtraLarge, FlagChargeLocation.Centre), 4)
            .Add(FlagChargeCreator.CircleChargeDist(backgroundColours: [colour], FlagChargeSize.ExtraLarge, FlagChargeLocation.Centre), 4)
            .Add(FlagChargeCreator.PlusChargeDist(backgroundColours: [colour], FlagChargeSize.ExtraLarge, FlagChargeLocation.Centre), 1)
            .Add(FlagChargeCreator.ShieldChargeDist(backgroundColours: [colour], FlagChargeSize.ExtraLarge, FlagChargeLocation.Centre), 1)
            .Add(FlagChargeCreator.CrescentChargeDist(backgroundColours: [colour], FlagChargeSize.ExtraLarge, FlagChargeLocation.Centre), 2)
            .Build()
            .Flatten();

    private static IDistribution<IReadOnlyList<FlagCharge>> TopLeftCornerChargesDist(FlagColour colour) =>
        WeightedDiscreteDistributionBuilder<IDistribution<IReadOnlyList<FlagCharge>>>.Empty()
            .Add(FlagChargeCreator.StarChargeDist(backgroundColours: [colour], FlagChargeSize.Medium, FlagChargeLocation.TopLeftCorner), 4)
            .Add(FlagChargeCreator.PlusChargeDist(backgroundColours: [colour], FlagChargeSize.Medium, FlagChargeLocation.TopLeftCorner), 1)
            .Add(FlagChargeCreator.ShieldChargeDist(backgroundColours: [colour], FlagChargeSize.Medium, FlagChargeLocation.TopLeftCorner), 1)
            .Add(FlagChargeCreator.CrescentChargeDist(backgroundColours: [colour], FlagChargeSize.Medium, FlagChargeLocation.TopLeftCorner), 4)
            .Build()
            .Flatten();
    
    private enum SolidFlagChargeLocation { None, Centre, TopLeftCorner }
}