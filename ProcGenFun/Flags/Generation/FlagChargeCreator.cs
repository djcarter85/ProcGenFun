namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Flags.Model;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class FlagChargeCreator
{
    public static IDistribution<IReadOnlyList<FlagCharge>> ChargesDist(FlagChargeShape.Type? chargeType,
        FlagColour backgroundColour, float size) =>
        chargeType switch
        {
            null => Singleton.New<IReadOnlyList<FlagCharge>>([]),
            FlagChargeShape.Type.Star => StarChargeDist(backgroundColour, size),
            FlagChargeShape.Type.StarBand => StarBandChargeDist(backgroundColour, size),
            FlagChargeShape.Type.Circle => CircleChargeDist(backgroundColour, size),
            _ => throw new ArgumentOutOfRangeException(nameof(chargeType), chargeType, null)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>> StarChargeDist(FlagColour backgroundColour, float size) =>
        from colour in FlagColours.AllowedAdjacentToDist(backgroundColour)
        select (IReadOnlyList<FlagCharge>)new List<FlagCharge>
        {
            new(
                new FlagChargeShape.Star(colour),
                size,
                FlagChargeHorizontalLocation.Centre,
                FlagChargeVerticalLocation.Centre)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>>
        StarBandChargeDist(FlagColour backgroundColour, float size) =>
        from colour in FlagColours.AllowedAdjacentToDist(backgroundColour)
        from count in Uniform.NewInclusive(1, 4)
        select (IReadOnlyList<FlagCharge>)new List<FlagCharge>
        {
            new(
                new FlagChargeShape.StarBand(colour, count), 
                size, 
                FlagChargeHorizontalLocation.Centre,
                FlagChargeVerticalLocation.Centre)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>> CircleChargeDist(FlagColour backgroundColour, float size) =>
        from colour in FlagColours.AllowedAdjacentToDist(backgroundColour)
        select (IReadOnlyList<FlagCharge>)new List<FlagCharge>
        {
            new(new FlagChargeShape.Circle(colour),
                size,
                FlagChargeHorizontalLocation.Centre,
                FlagChargeVerticalLocation.Centre)
        };
}