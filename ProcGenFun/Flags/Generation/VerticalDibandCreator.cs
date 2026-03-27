namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class VerticalDibandCreator
{
    public static IDistribution<Flag> Dist() =>
        from left in FlagColours.AllDist()
        from right in FlagColours.AllowedAdjacentToDist(left)
        from chargeLocation in ChargeLocationDist()
        from charges in ChargesDist(chargeLocation, left, right)
        select new Flag(new FlagPattern.VerticalDiband(left, right), charges);

    private static IDistribution<FlagChargeLocation?> ChargeLocationDist() =>
        WeightedDiscreteDistributionBuilder<FlagChargeLocation?>.Empty()
            .Add(null, 4)
            .Add(FlagChargeLocation.CentreLeft, 1)
            .Add(FlagChargeLocation.CentreRight, 1)
            .Build();

    private static IDistribution<IReadOnlyList<FlagCharge>> ChargesDist(
        FlagChargeLocation? chargeLocation, FlagColour left, FlagColour right) =>
        chargeLocation switch
        {
            FlagChargeLocation.CentreLeft =>
                from chargeType in ChargeTypeDist()
                from charges in FlagChargeCreator.ChargesDist(chargeType, [left], FlagChargeSize.Medium, FlagChargeLocation.CentreLeft)
                select charges,
            FlagChargeLocation.CentreRight =>
                from chargeType in ChargeTypeDist()
                from charges in FlagChargeCreator.ChargesDist(chargeType, [right], FlagChargeSize.Medium, FlagChargeLocation.CentreRight)
                select charges,
            null => Singleton.New<IReadOnlyList<FlagCharge>>([]),
            _ => throw new ArgumentOutOfRangeException(nameof(chargeLocation), chargeLocation, null)
        };

    private static IDistribution<FlagChargeShape.Type> ChargeTypeDist() =>
        WeightedDiscreteDistributionBuilder<FlagChargeShape.Type>.Empty()
            .Add(FlagChargeShape.Type.Star, 5)
            .Add(FlagChargeShape.Type.Circle, 1)
            .Add(FlagChargeShape.Type.Plus, 1)
            .Build();
}