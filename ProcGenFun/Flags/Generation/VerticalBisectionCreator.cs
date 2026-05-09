namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class VerticalBisectionCreator
{
    public static IDistribution<Flag> Dist() =>
        from left in FlagColours.AllDist()
        from right in FlagColours.AllowedAdjacentToDist(left)
        from chargeLocation in ChargeLocationDist()
        from charges in ChargesDist(chargeLocation, left, right)
        select new Flag(new FlagPattern.VerticalBisection(left, right), charges);

    private static IDistribution<FlagChargeLocation?> ChargeLocationDist() =>
        WeightedDiscreteDistributionBuilder<FlagChargeLocation?>.Empty()
            .Add(null, 4)
            .Add(FlagChargeLocation.CentreLeftHalf, 1)
            .Add(FlagChargeLocation.CentreRightHalf, 1)
            .Build();

    private static IDistribution<IReadOnlyList<FlagCharge>> ChargesDist(
        FlagChargeLocation? chargeLocation, FlagColour left, FlagColour right) =>
        chargeLocation switch
        {
            FlagChargeLocation.CentreLeftHalf => ChargesDist(left, FlagChargeLocation.CentreLeftHalf),
            FlagChargeLocation.CentreRightHalf => ChargesDist(right, FlagChargeLocation.CentreRightHalf),
            null => Singleton.New<IReadOnlyList<FlagCharge>>([]),
            _ => throw new ArgumentOutOfRangeException(nameof(chargeLocation), chargeLocation, null)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>> ChargesDist(FlagColour backgroundColour, FlagChargeLocation location) =>
        WeightedDiscreteDistributionBuilder<IDistribution<IReadOnlyList<FlagCharge>>>.Empty()
            .Add(FlagChargeCreator.StarChargeDist([backgroundColour], FlagChargeSize.Medium, location), 5)
            .Add(FlagChargeCreator.CircleChargeDist([backgroundColour], FlagChargeSize.Medium, location), 1)
            .Add(FlagChargeCreator.PlusChargeDist([backgroundColour], FlagChargeSize.Medium, location), 1)
            .Build()
            .Flatten();
}