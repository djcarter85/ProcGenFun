namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class HorizontalDibandCreator
{
    public static IDistribution<Flag> Dist() =>
        from top in FlagColours.AllDist()
        from bottom in FlagColours.AllowedAdjacentToDist(top)
        from decoration in DecorationDist([top, bottom])
        from chargeType in ChargeTypeDist()
        from charges in ChargesDist(chargeType, [top, bottom, GetDecorationColour(decoration)])
        select new Flag(new FlagPattern.HorizontalDiband(top, bottom, decoration), charges);

    private static FlagColour? GetDecorationColour(HorizontalDibandDecoration decoration) =>
        decoration switch
        {
            HorizontalDibandDecoration.None => null,
            HorizontalDibandDecoration.Fimbriation fimbriation => fimbriation.Colour,
        };

    private static IDistribution<HorizontalDibandDecoration> DecorationDist(IEnumerable<FlagColour> adjacentColours) =>
        from type in DecorationTypeDist()
        from fimbriation in DecorationDist(type, adjacentColours)
        select fimbriation;

    private static IDistribution<HorizontalDibandDecoration.Type> DecorationTypeDist() =>
        WeightedDiscreteDistributionBuilder<HorizontalDibandDecoration.Type>.Empty()
            .Add(HorizontalDibandDecoration.Type.None, 4)
            .Add(HorizontalDibandDecoration.Type.Fimbriation, 1)
            .Build();

    private static IDistribution<HorizontalDibandDecoration> DecorationDist(
        HorizontalDibandDecoration.Type type, IEnumerable<FlagColour> adjacentColours) =>
        type switch
        {
            HorizontalDibandDecoration.Type.None => Singleton.New<HorizontalDibandDecoration>(new HorizontalDibandDecoration.None()),
            HorizontalDibandDecoration.Type.Fimbriation => 
                from colour in FlagColours.AllowedAdjacentToDist(adjacentColours)
                select (HorizontalDibandDecoration)new HorizontalDibandDecoration.Fimbriation(colour),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

    private static IDistribution<FlagChargeShape.Type?> ChargeTypeDist() =>
        WeightedDiscreteDistributionBuilder<FlagChargeShape.Type?>.Empty()
            .Add(null, 10)
            .Add(FlagChargeShape.Type.Star, 3)
            .Add(FlagChargeShape.Type.Circle, 2)
            .Build();

    private static IDistribution<IReadOnlyList<FlagCharge>> ChargesDist(FlagChargeShape.Type? chargeType, IEnumerable<FlagColour?> adjacentColours) =>
        FlagChargeCreator.ChargesDist(
            chargeType,
            adjacentColours.ExcludingNull(),
            size: FlagChargeSize.Large,
            FlagChargeLocation.Centre);
}