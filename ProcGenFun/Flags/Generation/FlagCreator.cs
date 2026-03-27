namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Extensions;

public static class FlagCreator
{
    public static IDistribution<Flag> FlagDist() =>
        from flagType in FlagTypeDist()
        from flag in FlagDist(flagType)
        select flag;

    private static IDistribution<FlagPattern.Type> FlagTypeDist() =>
        WeightedDiscreteDistributionBuilder<FlagPattern.Type>.Empty()
            .Add(FlagPattern.Type.Solid, 15)
            .Add(FlagPattern.Type.Canton, 15)
            .Add(FlagPattern.Type.VerticalDiband, 25)
            .Add(FlagPattern.Type.HorizontalDiband, 35)
            .Add(FlagPattern.Type.VerticalTriband, 50)
            .Add(FlagPattern.Type.HorizontalTriband, 45)
            .Add(FlagPattern.Type.DiagonalBicolour, 5)
            .Add(FlagPattern.Type.Cross, 25)
            .Add(FlagPattern.Type.Saltire, 15)
            .Add(FlagPattern.Type.Quartered, 10)
            .Add(FlagPattern.Type.HorizontalStriped, 10)
            .Add(FlagPattern.Type.Pall, 15)
            .Add(FlagPattern.Type.PartyPerPall, 15)
            .Add(FlagPattern.Type.Rays, 10)
            .Build();

    private static IDistribution<Flag> FlagDist(FlagPattern.Type flagType) =>
        flagType switch
        {
            FlagPattern.Type.Solid => SolidFlagCreator.Dist(),
            FlagPattern.Type.Canton => CantonFlagCreator.Dist(),
            FlagPattern.Type.VerticalDiband => VerticalDibandCreator.Dist(),
            FlagPattern.Type.HorizontalDiband => HorizontalDibandCreator.Dist(),
            FlagPattern.Type.VerticalTriband => VerticalTribandCreator.Dist(),
            FlagPattern.Type.HorizontalTriband => HorizontalTribandCreator.Dist(),
            FlagPattern.Type.DiagonalBicolour => DiagonalBicolourCreator.Dist(),
            FlagPattern.Type.Cross => CrossCreator.Dist(),
            FlagPattern.Type.Saltire => SaltireCreator.Dist(),
            FlagPattern.Type.Quartered => QuarteredCreator.Dist(),
            FlagPattern.Type.HorizontalStriped => HorizontalStripedCreator.Dist(),
            FlagPattern.Type.Pall => PallCreator.Dist(),
            FlagPattern.Type.PartyPerPall => PartyPerPallCreator.Dist(),
            FlagPattern.Type.Rays => RaysCreator.Dist(),
            _ => throw new ArgumentOutOfRangeException(nameof(flagType), flagType, null)
        };
}