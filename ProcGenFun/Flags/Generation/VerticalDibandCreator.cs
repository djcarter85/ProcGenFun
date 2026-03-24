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

    private static IDistribution<FlagChargeHorizontalLocation?> ChargeLocationDist() =>
        WeightedDiscreteDistributionBuilder<FlagChargeHorizontalLocation?>.Empty()
            .Add(null, 4)
            .Add(FlagChargeHorizontalLocation.Left, 1)
            .Add(FlagChargeHorizontalLocation.Right, 1)
            .Build();

    private static IDistribution<IReadOnlyList<FlagCharge>> ChargesDist(
        FlagChargeHorizontalLocation? chargeLocation, FlagColour left, FlagColour right) =>
        chargeLocation switch
        {
            FlagChargeHorizontalLocation.Left =>
                from chargeType in ChargeTypeDist()
                from charges in FlagChargeCreator.ChargesDist(chargeType, [left], FlagChargeSize.Small, FlagChargeHorizontalLocation.Left, FlagChargeVerticalLocation.Centre)
                select charges,
            FlagChargeHorizontalLocation.Centre =>
                throw new ArgumentOutOfRangeException(nameof(chargeLocation), chargeLocation, null),
            FlagChargeHorizontalLocation.Right =>
                from chargeType in ChargeTypeDist()
                from charges in FlagChargeCreator.ChargesDist(chargeType, [right], FlagChargeSize.Small, FlagChargeHorizontalLocation.Right, FlagChargeVerticalLocation.Centre)
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