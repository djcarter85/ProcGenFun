namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class VerticalTribandCreator
{
    public static IDistribution<Flag> Dist() =>
        from left in FlagColours.AllDist()
        from middle in FlagColours.AllowedAdjacentToDist(left)
        from leftAndRightAreSame in Bernoulli.FromRatio(2, 5)
        from right in RightColourDist(leftAndRightAreSame, left, middle)
        from charges in ChargesDist(middle)
        select new Flag(new FlagPattern.VerticalTriband(left, middle, right), charges);

    private static IDistribution<FlagColour> RightColourDist(
        bool leftAndRightAreSame, FlagColour left, FlagColour middle) =>
        leftAndRightAreSame
            ? Singleton.New(left)
            : FlagColours.AllowedAdjacentToExceptingDist(adjacentColour: middle, exceptColour: left);

    private static IDistribution<IReadOnlyList<FlagCharge>> ChargesDist(FlagColour backgroundColour) =>
        WeightedDiscreteDistributionBuilder<IDistribution<IReadOnlyList<FlagCharge>>>.Empty()
            .Add(FlagChargeCreator.NoChargesDist(), 3)
            .Add(FlagChargeCreator.StarChargeDist([backgroundColour], FlagChargeSize.Medium, FlagChargeLocation.Centre), 1)
            .Build()
            .Flatten();
}