namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class DiagonalBisectionCreator
{
    public static IDistribution<Flag> Dist() =>
        from left in FlagColours.AllDist()
        from right in FlagColours.AllowedAdjacentToDist(left)
        from diagonal in UniformDistribution.Create([Diagonal.Down, Diagonal.Up])
        from decoration in DecorationDist(left, right)
        select new Flag(new FlagPattern.DiagonalBisection(left, right, diagonal, decoration), []);

    private static IDistribution<DiagonalBisectionDecoration> DecorationDist(
        FlagColour left, FlagColour right) =>
        WeightedDiscreteDistributionBuilder<IDistribution<DiagonalBisectionDecoration>>.Empty()
            .Add(NoDecorationDist(), 3)
            .Add(LeftRayDist(left), 1)
            .Add(RightRayDist(right), 1)
            .Build()
            .Flatten();

    private static IDistribution<DiagonalBisectionDecoration> NoDecorationDist() => 
        Singleton.New<DiagonalBisectionDecoration>(new DiagonalBisectionDecoration.None());

    private static IDistribution<DiagonalBisectionDecoration> LeftRayDist(FlagColour left) => 
        from colour in FlagColours.AllowedAdjacentToDist(left) 
        select (DiagonalBisectionDecoration)new DiagonalBisectionDecoration.LeftRay(colour);

    private static IDistribution<DiagonalBisectionDecoration> RightRayDist(FlagColour right) => 
        from colour in FlagColours.AllowedAdjacentToDist(right)
        select (DiagonalBisectionDecoration)new DiagonalBisectionDecoration.RightRay(colour);
}