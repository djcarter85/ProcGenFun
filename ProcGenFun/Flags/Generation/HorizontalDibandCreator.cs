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
        from fimbriation in FimbriationDist([top, bottom])
        from chargeType in ChargeTypeDist()
        from charges in ChargesDist(chargeType, [top, bottom, fimbriation])
        select new Flag(new FlagPattern.HorizontalDiband(top, bottom, fimbriation), charges);

    private static IDistribution<FlagColour?> FimbriationDist(IEnumerable<FlagColour> adjacentColours) =>
        from shouldFimbriate in Bernoulli.FromRatio(1, 5)
        from fimbriation in FimbriationDist(shouldFimbriate, adjacentColours)
        select fimbriation;

    private static IDistribution<FlagColour?> FimbriationDist(
        bool shouldFimbriate, IEnumerable<FlagColour> adjacentColours) =>
        shouldFimbriate
            ? FlagColours.AllowedAdjacentToDist(adjacentColours).Nullable()
            : Singleton.New<FlagColour?>(null);

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
            FlagChargeHorizontalLocation.Centre,
            FlagChargeVerticalLocation.Centre);
}