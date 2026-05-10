namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class HorizontalTribandCreator
{
    public static IDistribution<Flag> Dist() =>
        from top in FlagColours.AllDist()
        from middle in FlagColours.AllowedAdjacentToDist(top)
        from topAndBottomAreSame in Bernoulli.FromRatio(2, 5)
        from bottom in BottomColourDist(topAndBottomAreSame, top, middle)
        from sizing in SizingDist()
        from fimbriation in FimbriationDist([top, middle, bottom])
        from charges in ChargesDist(middle, sizing)
        select new Flag(new FlagPattern.HorizontalTriband(top, middle, bottom, sizing, fimbriation), charges);

    private static IDistribution<HorizontalTribandSizing> SizingDist() =>
        WeightedDiscreteDistributionBuilder<HorizontalTribandSizing>.Empty()
            .Add(HorizontalTribandSizing.Equal, 10)
            .Add(HorizontalTribandSizing.LargeMiddle, 1)
            .Add(HorizontalTribandSizing.SmallMiddle, 1)
            .Add(HorizontalTribandSizing.LargeTop, 1)
            .Build();

    private static IDistribution<FlagColour> BottomColourDist(
        bool topAndBottomAreSame, FlagColour top, FlagColour middle) =>
        topAndBottomAreSame
            ? Singleton.New(top)
            : FlagColours.AllowedAdjacentToExceptingDist(adjacentColour: middle, exceptColour: top);

    private static IDistribution<FlagColour?> FimbriationDist(IEnumerable<FlagColour> adjacentColours) =>
        from shouldFimbriate in Bernoulli.FromRatio(1, 5)
        from fimbriation in FimbriationDist(shouldFimbriate, adjacentColours)
        select fimbriation;

    private static IDistribution<FlagColour?> FimbriationDist(
        bool shouldFimbriate, IEnumerable<FlagColour> adjacentColours) =>
        shouldFimbriate
            ? FlagColours.AllowedAdjacentToDist(adjacentColours).Nullable()
            : Singleton.New<FlagColour?>(null);

    private static IDistribution<IReadOnlyList<FlagCharge>> ChargesDist(
        FlagColour middle, HorizontalTribandSizing sizing) =>
        sizing switch
        {
            HorizontalTribandSizing.Equal => ChargesDist(middle, FlagChargeSize.Small),
            HorizontalTribandSizing.LargeMiddle => ChargesDist(middle, FlagChargeSize.Medium),
            HorizontalTribandSizing.SmallMiddle => FlagChargeCreator.NoChargesDist(),
            HorizontalTribandSizing.LargeTop => FlagChargeCreator.NoChargesDist(),
            _ => throw new ArgumentOutOfRangeException(nameof(sizing), sizing, null)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>> ChargesDist(FlagColour middle, FlagChargeSize chargeSize) =>
        WeightedDiscreteDistributionBuilder<IDistribution<IReadOnlyList<FlagCharge>>>.Empty()
            .Add(FlagChargeCreator.NoChargesDist(), 3)
            .Add(FlagChargeCreator.StarBandChargeDist([middle], chargeSize, FlagChargeLocation.Centre), 1)
            .Build()
            .Flatten();
}