namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class HorizontalBisectionCreator
{
    public static IDistribution<Flag> Dist() =>
        from top in FlagColours.AllDist()
        from bottom in FlagColours.AllowedAdjacentToDist(top)
        from decoration in DecorationDist([top, bottom])
        from charges in ChargeTypeDist(GetChargeAdjacentColours(top, bottom, decoration), GetChargeSize(decoration), GetChargeLocation(decoration))
        select new Flag(new FlagPattern.HorizontalBisection(top, bottom, decoration), charges);

    private static FlagChargeSize GetChargeSize(HorizontalBisectionDecoration decoration) =>
        decoration switch
        {
            HorizontalBisectionDecoration.None => FlagChargeSize.Large,
            HorizontalBisectionDecoration.Fimbriation => FlagChargeSize.Large,
            HorizontalBisectionDecoration.Pile => FlagChargeSize.Small,
            HorizontalBisectionDecoration.VerticalBand => FlagChargeSize.Small
        };

    private static FlagChargeLocation GetChargeLocation(HorizontalBisectionDecoration decoration) =>
        decoration switch
        {
            HorizontalBisectionDecoration.None => FlagChargeLocation.Centre,
            HorizontalBisectionDecoration.Fimbriation => FlagChargeLocation.Centre,
            HorizontalBisectionDecoration.Pile => FlagChargeLocation.CentreLeftThird,
            HorizontalBisectionDecoration.VerticalBand => FlagChargeLocation.CentreLeftThird
        };

    private static IReadOnlyList<FlagColour> GetChargeAdjacentColours(FlagColour top, FlagColour bottom,
        HorizontalBisectionDecoration decoration) =>
        decoration switch
        {
            HorizontalBisectionDecoration.None => [top, bottom],
            HorizontalBisectionDecoration.Fimbriation fimbriation => [top, bottom, fimbriation.Colour],
            HorizontalBisectionDecoration.Pile pile => [pile.Colour],
            HorizontalBisectionDecoration.VerticalBand verticalBand => [verticalBand.Colour]
        };

    private static IDistribution<HorizontalBisectionDecoration> DecorationDist(IReadOnlyList<FlagColour> adjacentColours) =>
        WeightedDiscreteDistributionBuilder<IDistribution<HorizontalBisectionDecoration>>.Empty()
            .Add(NoDecorationDist(), 6)
            .Add(FimbriationDist(adjacentColours), 1)
            .Add(PileDist(adjacentColours), 2)
            .Add(VerticalBandDist(adjacentColours), 2)
            .Build()
            .Flatten();

    private static Singleton<HorizontalBisectionDecoration> NoDecorationDist() => 
        Singleton.New<HorizontalBisectionDecoration>(new HorizontalBisectionDecoration.None());

    private static IDistribution<HorizontalBisectionDecoration> FimbriationDist(IEnumerable<FlagColour> adjacentColours) =>
        from colour in FlagColours.AllowedAdjacentToDist(adjacentColours)
        select (HorizontalBisectionDecoration)new HorizontalBisectionDecoration.Fimbriation(colour);

    private static IDistribution<HorizontalBisectionDecoration> PileDist(IEnumerable<FlagColour> adjacentColours) =>
        from colour in FlagColours.AllowedAdjacentToDist(adjacentColours)
        select (HorizontalBisectionDecoration)new HorizontalBisectionDecoration.Pile(colour);

    private static IDistribution<HorizontalBisectionDecoration> VerticalBandDist(IEnumerable<FlagColour> adjacentColours) =>
        from colour in FlagColours.AllowedAdjacentToDist(adjacentColours)
        select (HorizontalBisectionDecoration)new HorizontalBisectionDecoration.VerticalBand(colour);

    private static IDistribution<IReadOnlyList<FlagCharge>> ChargeTypeDist(
        IReadOnlyList<FlagColour> backgroundColours,
        FlagChargeSize size,
        FlagChargeLocation location) =>
        WeightedDiscreteDistributionBuilder<IDistribution<IReadOnlyList<FlagCharge>>>.Empty()
            .Add(FlagChargeCreator.NoChargesDist(), 10)
            .Add(FlagChargeCreator.StarChargeDist(backgroundColours, size, location), 3)
            .Add(FlagChargeCreator.CircleChargeDist(backgroundColours, size, location), 2)
            .Build()
            .Flatten();
}