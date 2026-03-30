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
        FlagChargeLocation location) =>
        chargeType switch
        {
            null => Singleton.New<IReadOnlyList<FlagCharge>>([]),
            FlagChargeShape.Type.Star => StarChargeDist(backgroundColours, size, location),
            FlagChargeShape.Type.StarBand => StarBandChargeDist(backgroundColours, size, location),
            FlagChargeShape.Type.Circle => CircleChargeDist(backgroundColours, size, location),
            FlagChargeShape.Type.Plus => PlusChargeDist(backgroundColours, size, location),
            FlagChargeShape.Type.Shield => ShieldChargeDist(backgroundColours, size, location),
            _ => throw new ArgumentOutOfRangeException(nameof(chargeType), chargeType, null)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>> StarChargeDist(
        IEnumerable<FlagColour> backgroundColours,
        FlagChargeSize size,
        FlagChargeLocation location) =>
        from colour in FlagColours.AllowedAdjacentToDist(backgroundColours)
        select (IReadOnlyList<FlagCharge>)new List<FlagCharge>
        {
            new(
                new FlagChargeShape.Star(colour),
                size,
                location)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>> StarBandChargeDist(
        IEnumerable<FlagColour> backgroundColours,
        FlagChargeSize size,
        FlagChargeLocation location) =>
        from colour in FlagColours.AllowedAdjacentToDist(backgroundColours)
        from count in Uniform.NewInclusive(1, 4)
        select (IReadOnlyList<FlagCharge>)new List<FlagCharge>
        {
            new(
                new FlagChargeShape.StarBand(colour, count),
                size,
                location)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>> CircleChargeDist(
        IEnumerable<FlagColour> backgroundColours,
        FlagChargeSize size,
        FlagChargeLocation location) =>
        from colour in FlagColours.AllowedAdjacentToDist(backgroundColours)
        select (IReadOnlyList<FlagCharge>)new List<FlagCharge>
        {
            new(new FlagChargeShape.Circle(colour),
                size,
                location)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>> PlusChargeDist(
        IEnumerable<FlagColour> backgroundColours,
        FlagChargeSize size,
        FlagChargeLocation location) =>
        from colour in FlagColours.AllowedAdjacentToDist(backgroundColours)
        select (IReadOnlyList<FlagCharge>)new List<FlagCharge>
        {
            new(new FlagChargeShape.Plus(colour),
                size,
                location)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>> ShieldChargeDist(
        IEnumerable<FlagColour> backgroundColours,
        FlagChargeSize size,
        FlagChargeLocation location) =>
        from colour in FlagColours.AllowedAdjacentToDist(backgroundColours)
        select (IReadOnlyList<FlagCharge>)new List<FlagCharge>
        {
            new(new FlagChargeShape.Shield(colour),
                size,
                location)
        };
}