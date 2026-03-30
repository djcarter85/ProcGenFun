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
        from charges in FlagChargeCreator.ChargesDist(chargeType, GetChargeAdjacentColours(top, bottom, decoration), GetChargeSize(decoration), GetChargeLocation(decoration))
        select new Flag(new FlagPattern.HorizontalDiband(top, bottom, decoration), charges);

    private static FlagChargeSize GetChargeSize(HorizontalDibandDecoration decoration) =>
        decoration switch
        {
            HorizontalDibandDecoration.None => FlagChargeSize.Large,
            HorizontalDibandDecoration.Fimbriation => FlagChargeSize.Large,
            HorizontalDibandDecoration.Pile => FlagChargeSize.Small,
            HorizontalDibandDecoration.VerticalBand => FlagChargeSize.Small
        };

    private static FlagChargeLocation GetChargeLocation(HorizontalDibandDecoration decoration) =>
        decoration switch
        {
            HorizontalDibandDecoration.None => FlagChargeLocation.Centre,
            HorizontalDibandDecoration.Fimbriation => FlagChargeLocation.Centre,
            HorizontalDibandDecoration.Pile => FlagChargeLocation.CentreLeftThird,
            HorizontalDibandDecoration.VerticalBand => FlagChargeLocation.CentreLeftThird
        };

    private static IEnumerable<FlagColour> GetChargeAdjacentColours(FlagColour top, FlagColour bottom,
        HorizontalDibandDecoration decoration) =>
        decoration switch
        {
            HorizontalDibandDecoration.None => [top, bottom],
            HorizontalDibandDecoration.Fimbriation fimbriation => [top, bottom, fimbriation.Colour],
            HorizontalDibandDecoration.Pile pile => [pile.Colour],
            HorizontalDibandDecoration.VerticalBand verticalBand => [verticalBand.Colour]
        };

    private static IDistribution<HorizontalDibandDecoration> DecorationDist(IReadOnlyList<FlagColour> adjacentColours) =>
        WeightedDiscreteDistributionBuilder<IDistribution<HorizontalDibandDecoration>>.Empty()
            .Add(NoDecorationDist(), 6)
            .Add(FimbriationDist(adjacentColours), 1)
            .Add(PileDist(adjacentColours), 2)
            .Add(VerticalBandDist(adjacentColours), 2)
            .Build()
            .Flatten();

    private static Singleton<HorizontalDibandDecoration> NoDecorationDist() => 
        Singleton.New<HorizontalDibandDecoration>(new HorizontalDibandDecoration.None());

    private static IDistribution<HorizontalDibandDecoration> FimbriationDist(IEnumerable<FlagColour> adjacentColours) =>
        from colour in FlagColours.AllowedAdjacentToDist(adjacentColours)
        select (HorizontalDibandDecoration)new HorizontalDibandDecoration.Fimbriation(colour);

    private static IDistribution<HorizontalDibandDecoration> PileDist(IEnumerable<FlagColour> adjacentColours) =>
        from colour in FlagColours.AllowedAdjacentToDist(adjacentColours)
        select (HorizontalDibandDecoration)new HorizontalDibandDecoration.Pile(colour);

    private static IDistribution<HorizontalDibandDecoration> VerticalBandDist(IEnumerable<FlagColour> adjacentColours) =>
        from colour in FlagColours.AllowedAdjacentToDist(adjacentColours)
        select (HorizontalDibandDecoration)new HorizontalDibandDecoration.VerticalBand(colour);

    private static IDistribution<FlagChargeShape.Type?> ChargeTypeDist() =>
        WeightedDiscreteDistributionBuilder<FlagChargeShape.Type?>.Empty()
            .Add(null, 10)
            .Add(FlagChargeShape.Type.Star, 3)
            .Add(FlagChargeShape.Type.Circle, 2)
            .Build();
}