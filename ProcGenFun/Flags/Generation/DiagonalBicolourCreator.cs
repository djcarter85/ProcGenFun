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
        from decoration in DecorationDist(left)
        select new Flag(new FlagPattern.DiagonalBicolour(left, right, diagonal, decoration), []);

    private static IDistribution<DiagonalBicolourDecoration> DecorationDist(FlagColour left) =>
        from type in DecorationTypeDist()
        from decoration in DecorationDist(type, left)
        select decoration;

    private static IDistribution<DiagonalBicolourDecoration> DecorationDist(
        DiagonalBicolourDecoration.Type type, FlagColour left) =>
        type switch
        {
            DiagonalBicolourDecoration.Type.None => Singleton.New<DiagonalBicolourDecoration>(new DiagonalBicolourDecoration.None()),
            DiagonalBicolourDecoration.Type.LeftRay => 
                from colour in FlagColours.AllowedAdjacentToDist(left)
                select (DiagonalBicolourDecoration)new DiagonalBicolourDecoration.LeftRay(colour),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

    private static IDistribution<DiagonalBicolourDecoration.Type> DecorationTypeDist() =>
        WeightedDiscreteDistributionBuilder<DiagonalBicolourDecoration.Type>.Empty()
            .Add(DiagonalBicolourDecoration.Type.None, 3)
            .Add(DiagonalBicolourDecoration.Type.LeftRay, 1)
            .Build();
}