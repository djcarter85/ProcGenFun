namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Flags.Model;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class FlagChargeCreator
{
    public static IDistribution<IReadOnlyList<FlagCharge>> ChargesDist(
        FlagChargeShape.Type? chargeType,
        IEnumerable<FlagColour> backgroundColours,
        FlagChargeSize size,
        FlagChargeHorizontalLocation horizontalLocation,
        FlagChargeVerticalLocation verticalLocation) =>
        chargeType switch
        {
            null => Singleton.New<IReadOnlyList<FlagCharge>>([]),
            FlagChargeShape.Type.Star => StarChargeDist(backgroundColours, size, horizontalLocation, verticalLocation),
            FlagChargeShape.Type.StarBand => StarBandChargeDist(backgroundColours, size, horizontalLocation, verticalLocation),
            FlagChargeShape.Type.Circle => CircleChargeDist(backgroundColours, size, horizontalLocation, verticalLocation),
            FlagChargeShape.Type.Plus => PlusChargeDist(backgroundColours, size, horizontalLocation, verticalLocation),
            _ => throw new ArgumentOutOfRangeException(nameof(chargeType), chargeType, null)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>> StarChargeDist(
        IEnumerable<FlagColour> backgroundColours,
        FlagChargeSize size,
        FlagChargeHorizontalLocation horizontalLocation,
        FlagChargeVerticalLocation verticalLocation) =>
        from colour in FlagColours.AllowedAdjacentToDist(backgroundColours)
        select (IReadOnlyList<FlagCharge>)new List<FlagCharge>
        {
            new(
                new FlagChargeShape.Star(colour),
                size,
                horizontalLocation,
                verticalLocation)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>> StarBandChargeDist(
        IEnumerable<FlagColour> backgroundColours,
        FlagChargeSize size,
        FlagChargeHorizontalLocation horizontalLocation,
        FlagChargeVerticalLocation verticalLocation) =>
        from colour in FlagColours.AllowedAdjacentToDist(backgroundColours)
        from count in Uniform.NewInclusive(1, 4)
        select (IReadOnlyList<FlagCharge>)new List<FlagCharge>
        {
            new(
                new FlagChargeShape.StarBand(colour, count),
                size,
                horizontalLocation,
                verticalLocation)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>> CircleChargeDist(
        IEnumerable<FlagColour> backgroundColours,
        FlagChargeSize size,
        FlagChargeHorizontalLocation horizontalLocation,
        FlagChargeVerticalLocation verticalLocation) =>
        from colour in FlagColours.AllowedAdjacentToDist(backgroundColours)
        select (IReadOnlyList<FlagCharge>)new List<FlagCharge>
        {
            new(new FlagChargeShape.Circle(colour),
                size,
                horizontalLocation,
                verticalLocation)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>> PlusChargeDist(
        IEnumerable<FlagColour> backgroundColours,
        FlagChargeSize size,
        FlagChargeHorizontalLocation horizontalLocation,
        FlagChargeVerticalLocation verticalLocation) =>
        from colour in FlagColours.AllowedAdjacentToDist(backgroundColours)
        select (IReadOnlyList<FlagCharge>)new List<FlagCharge>
        {
            new(new FlagChargeShape.Plus(colour),
                size,
                horizontalLocation,
                verticalLocation)
        };
}