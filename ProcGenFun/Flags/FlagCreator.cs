namespace ProcGenFun.Flags;

using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;
using System;
using System.ComponentModel;

public static class FlagCreator
{
    public static IDistribution<Flag> FlagDist() =>
        from flagType in UniformDistribution.Create(Flag.Types)
        from flag in FlagDist(flagType)
        select flag;

    private static IDistribution<Flag> FlagDist(Flag.Type flagType) =>
        flagType switch
        {
            Flag.Type.Solid => SolidFlagDist(),
            Flag.Type.VerticalTriband => VerticalTribandDist(),
            _ => throw new ArgumentOutOfRangeException(nameof(flagType), flagType, null)
        };

    private static IDistribution<Flag> SolidFlagDist() =>
        from colour in ColourDist() select (Flag)new Flag.Solid(colour);

    private static IDistribution<Flag> VerticalTribandDist() =>
        from left in ColourDist()
        from middle in ColourDist()
        from right in ColourDist()
        select (Flag)new Flag.VerticalTriband(left, middle, right);

    public static IDistribution<FlagColour> ColourDist() =>
        UniformDistribution.Create(
            [
                FlagColour.Red,
                FlagColour.Orange,
                FlagColour.Yellow,
                FlagColour.Green,
                FlagColour.LightBlue,
                FlagColour.DarkBlue,
                FlagColour.White,
                FlagColour.Black
            ]);
}