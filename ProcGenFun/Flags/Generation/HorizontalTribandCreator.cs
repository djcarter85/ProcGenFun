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
        from chargeType in ChargeTypeDist(sizing)
        from charge in FlagChargeCreator.ChargesDist(chargeType, backgroundColours: [middle], size: GetFlagChargeSize(sizing), FlagChargeLocation.Centre)
        select new Flag(new FlagPattern.HorizontalTriband(top, middle, bottom, sizing, fimbriation), charge);

    private static FlagChargeSize GetFlagChargeSize(HorizontalTribandSizing sizing) =>
        sizing switch
        {
            HorizontalTribandSizing.Equal => FlagChargeSize.Small,
            HorizontalTribandSizing.LargeMiddle => FlagChargeSize.Medium,
            HorizontalTribandSizing.SmallMiddle => FlagChargeSize.Small,
            _ => throw new ArgumentOutOfRangeException(nameof(sizing), sizing, null)
        };

    private static IDistribution<HorizontalTribandSizing> SizingDist() =>
        WeightedDiscreteDistributionBuilder<HorizontalTribandSizing>.Empty()
            .Add(HorizontalTribandSizing.Equal, 10)
            .Add(HorizontalTribandSizing.LargeMiddle, 1)
            .Add(HorizontalTribandSizing.SmallMiddle, 1)
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

    private static IDistribution<FlagChargeShape.Type?> ChargeTypeDist(HorizontalTribandSizing sizing) =>
        WeightedDiscreteDistributionBuilder<FlagChargeShape.Type?>.Empty()
            .Add(null, 3)
            .Add(FlagChargeShape.Type.StarBand, sizing == HorizontalTribandSizing.SmallMiddle ? 0 : 1)
            .Build();
}