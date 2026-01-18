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
        from colour in ColourDist() select new Flag(colour);

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