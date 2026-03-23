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
        FlagChargeHorizontalLocation horizontalLocation) =>
        chargeType switch
        {
            null => Singleton.New<IReadOnlyList<FlagCharge>>([]),
            FlagChargeShape.Type.Star => StarChargeDist(backgroundColours, size, horizontalLocation),
            FlagChargeShape.Type.StarBand => StarBandChargeDist(backgroundColours, size, horizontalLocation),
            FlagChargeShape.Type.Circle => CircleChargeDist(backgroundColours, size, horizontalLocation),
            FlagChargeShape.Type.Plus => PlusChargeDist(backgroundColours, size, horizontalLocation),
            _ => throw new ArgumentOutOfRangeException(nameof(chargeType), chargeType, null)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>> StarChargeDist(
        IEnumerable<FlagColour> backgroundColours, FlagChargeSize size, FlagChargeHorizontalLocation horizontalLocation) =>
        from colour in FlagColours.AllowedAdjacentToDist(backgroundColours)
        select (IReadOnlyList<FlagCharge>)new List<FlagCharge>
        {
            new(
                new FlagChargeShape.Star(colour),
                size,
                horizontalLocation,
                FlagChargeVerticalLocation.Centre)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>> StarBandChargeDist(
        IEnumerable<FlagColour> backgroundColours, FlagChargeSize size, FlagChargeHorizontalLocation horizontalLocation) =>
        from colour in FlagColours.AllowedAdjacentToDist(backgroundColours)
        from count in Uniform.NewInclusive(1, 4)
        select (IReadOnlyList<FlagCharge>)new List<FlagCharge>
        {
            new(
                new FlagChargeShape.StarBand(colour, count),
                size,
                horizontalLocation,
                FlagChargeVerticalLocation.Centre)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>> CircleChargeDist(
        IEnumerable<FlagColour> backgroundColours, FlagChargeSize size, FlagChargeHorizontalLocation horizontalLocation) =>
        from colour in FlagColours.AllowedAdjacentToDist(backgroundColours)
        select (IReadOnlyList<FlagCharge>)new List<FlagCharge>
        {
            new(new FlagChargeShape.Circle(colour),
                size,
                horizontalLocation,
                FlagChargeVerticalLocation.Centre)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>> PlusChargeDist(
        IEnumerable<FlagColour> backgroundColours, FlagChargeSize size, FlagChargeHorizontalLocation horizontalLocation) =>
        from colour in FlagColours.AllowedAdjacentToDist(backgroundColours)
        select (IReadOnlyList<FlagCharge>)new List<FlagCharge>
        {
            new(new FlagChargeShape.Plus(colour),
                size,
                horizontalLocation,
                FlagChargeVerticalLocation.Centre)
        };
}