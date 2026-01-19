namespace ProcGenFun.Flags;

using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public static class FlagCreator
{
    private static readonly IReadOnlyList<FlagColour> allColours =
    [
        FlagColour.Red,
        FlagColour.Orange,
        FlagColour.Yellow,
        FlagColour.Green,
        FlagColour.LightBlue,
        FlagColour.DarkBlue,
        FlagColour.White,
        FlagColour.Black
    ];

    public static IDistribution<Flag> FlagDist() =>
        from flagType in FlagTypeDist()
        from flag in FlagDist(flagType)
        select flag;

    private static IDistribution<Flag.Type> FlagTypeDist() => 
        WeightedDiscreteDistribution.New(
            [
                new Weighting<Flag.Type>(Flag.Type.Solid, 1),
                new Weighting<Flag.Type>(Flag.Type.VerticalDiband, 1),
                new Weighting<Flag.Type>(Flag.Type.HorizontalDiband, 1),
                new Weighting<Flag.Type>(Flag.Type.VerticalTriband, 1),
                new Weighting<Flag.Type>(Flag.Type.HorizontalTriband, 1),
            ]);

    private static IDistribution<Flag> FlagDist(Flag.Type flagType) =>
        flagType switch
        {
            Flag.Type.Solid => SolidFlagDist(),
            Flag.Type.VerticalDiband => VerticalDibandDist(),
            Flag.Type.HorizontalDiband => HorizontalDibandDist(),
            Flag.Type.VerticalTriband => VerticalTribandDist(),
            Flag.Type.HorizontalTriband => HorizontalTribandDist(),
            _ => throw new ArgumentOutOfRangeException(nameof(flagType), flagType, null)
        };

    private static IDistribution<Flag> SolidFlagDist() =>
        from colour in UniformDistribution.Create(allColours) select (Flag)new Flag.Solid(colour);

    private static IDistribution<Flag> VerticalDibandDist() =>
        from colours in Shuffle.New(allColours)
        select (Flag)new Flag.VerticalDiband(colours[0], colours[1]);

    private static IDistribution<Flag> HorizontalDibandDist() =>
        from colours in Shuffle.New(allColours)
        select (Flag)new Flag.HorizontalDiband(colours[0], colours[1]);

    private static IDistribution<Flag> VerticalTribandDist() =>
        from colours in Shuffle.New(allColours)
        select (Flag)new Flag.VerticalTriband(colours[0], colours[1], colours[2]);

    private static IDistribution<Flag> HorizontalTribandDist() =>
        from colours in Shuffle.New(allColours)
        select (Flag)new Flag.HorizontalTriband(colours[0], colours[1], colours[2]);
}