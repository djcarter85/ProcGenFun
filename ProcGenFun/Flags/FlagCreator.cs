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
                new Weighting<Flag.Type>(Flag.Type.VerticalDiband, 2),
                new Weighting<Flag.Type>(Flag.Type.HorizontalDiband, 2),
                new Weighting<Flag.Type>(Flag.Type.VerticalTriband, 3),
                new Weighting<Flag.Type>(Flag.Type.HorizontalTriband, 2),
                new Weighting<Flag.Type>(Flag.Type.Cross, 2),
            ]);

    private static IDistribution<Flag> FlagDist(Flag.Type flagType) =>
        flagType switch
        {
            Flag.Type.Solid => SolidFlagDist(),
            Flag.Type.VerticalDiband => VerticalDibandDist(),
            Flag.Type.HorizontalDiband => HorizontalDibandDist(),
            Flag.Type.VerticalTriband => VerticalTribandDist(),
            Flag.Type.HorizontalTriband => HorizontalTribandDist(),
            Flag.Type.Cross => CrossDist(),
            _ => throw new ArgumentOutOfRangeException(nameof(flagType), flagType, null)
        };

    private static IDistribution<Flag> SolidFlagDist() =>
        from colour in AllColoursDist()
        select (Flag)new Flag.Solid(colour);

    private static IDistribution<Flag> VerticalDibandDist() =>
        from left in AllColoursDist()
        from right in AllColoursExceptDist(left)
        select (Flag)new Flag.VerticalDiband(left, right);

    private static IDistribution<Flag> HorizontalDibandDist() =>
        from top in AllColoursDist()
        from bottom in AllColoursExceptDist(top)
        select (Flag)new Flag.HorizontalDiband(top, bottom);

    private static IDistribution<Flag> VerticalTribandDist() =>
        from left in AllColoursDist()
        from middle in AllColoursExceptDist(left)
        from right in AllColoursExceptDist(middle)
        select (Flag)new Flag.VerticalTriband(left, middle, right);

    private static IDistribution<Flag> HorizontalTribandDist() =>
        from top in AllColoursDist()
        from middle in AllColoursExceptDist(top)
        from bottom in AllColoursExceptDist(middle)
        select (Flag)new Flag.HorizontalTriband(top, middle, bottom);

    private static IDistribution<Flag> CrossDist() =>
        from background in AllColoursDist()
        from foreground in AllColoursExceptDist(background)
        select (Flag)new Flag.Cross(background, foreground);

    private static IDistribution<FlagColour> AllColoursDist() =>
        UniformDistribution.Create(allColours);

    private static IDistribution<FlagColour> AllColoursExceptDist(FlagColour exceptColour) =>
        UniformDistribution.Create(allColours.Except([exceptColour]));
}