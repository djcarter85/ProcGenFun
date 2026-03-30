namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class DiagonalBicolourCreator
{
    public static IDistribution<Flag> Dist() =>
        from left in FlagColours.AllDist()
        from right in FlagColours.AllowedAdjacentToDist(left)
        from diagonal in UniformDistribution.Create([Diagonal.Down, Diagonal.Up])
        from decoration in DecorationDist(left, right)
        select new Flag(new FlagPattern.DiagonalBicolour(left, right, diagonal, decoration), []);

    private static IDistribution<DiagonalBicolourDecoration> DecorationDist(
        FlagColour left, FlagColour right) =>
        WeightedDiscreteDistributionBuilder<IDistribution<DiagonalBicolourDecoration>>.Empty()
            .Add(NoDecorationDist(), 3)
            .Add(LeftRayDist(left), 1)
            .Add(RightRayDist(right), 1)
            .Build()
            .Flatten();

    private static IDistribution<DiagonalBicolourDecoration> NoDecorationDist() => 
        Singleton.New<DiagonalBicolourDecoration>(new DiagonalBicolourDecoration.None());

    private static IDistribution<DiagonalBicolourDecoration> LeftRayDist(FlagColour left) => 
        from colour in FlagColours.AllowedAdjacentToDist(left) 
        select (DiagonalBicolourDecoration)new DiagonalBicolourDecoration.LeftRay(colour);

    private static IDistribution<DiagonalBicolourDecoration> RightRayDist(FlagColour right) => 
        from colour in FlagColours.AllowedAdjacentToDist(right)
        select (DiagonalBicolourDecoration)new DiagonalBicolourDecoration.RightRay(colour);
}