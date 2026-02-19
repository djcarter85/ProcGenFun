namespace ProcGenFun.Flags;

using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using static Flag;
using static FlagCharge;

public static class FlagCreator
{
    private static readonly IEnumerable<Weighting<FlagColour>> allColourWeightings =
    [
        new(FlagColour.Red, 10),
        new(FlagColour.Orange, 3),
        new(FlagColour.Yellow, 6),
        new(FlagColour.Green, 10),
        new(FlagColour.LightBlue, 5),
        new(FlagColour.DarkBlue, 10),
        new(FlagColour.Burgundy, 2),
        new(FlagColour.Purple, 2),
        new(FlagColour.Grey, 1),
        new(FlagColour.White, 7),
        new(FlagColour.Black, 3)
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
                new Weighting<Flag.Type>(Flag.Type.HorizontalDiband, 3),
                new Weighting<Flag.Type>(Flag.Type.VerticalTriband, 4),
                new Weighting<Flag.Type>(Flag.Type.HorizontalTriband, 3),
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

    private static IDistribution<Flag> SolidFlagDist()
    {
        var chargeTypeDist = WeightedDiscreteDistribution.New(
        [
            new Weighting<FlagCharge.Type>(FlagCharge.Type.None, 1),
            new Weighting<FlagCharge.Type>(FlagCharge.Type.Star, 2),
        ]);
        
        return from colour in AllColoursDist()
            from chargeType in chargeTypeDist
            from charge in ChargeDist(chargeType, disallowedColour: colour)
            select (Flag)new Solid(colour, charge);
    }

    private static IDistribution<FlagCharge> ChargeDist(FlagCharge.Type chargeType, FlagColour disallowedColour) =>
        chargeType switch
        {
            FlagCharge.Type.None =>
                Singleton.New<FlagCharge>(new None()),
            FlagCharge.Type.Star =>
                from colour in AllColoursExceptDist(disallowedColour)
                select (FlagCharge)new Star(colour),
            FlagCharge.Type.StarBand =>
                from colour in AllColoursExceptDist(disallowedColour)
                select (FlagCharge)new StarBand(colour),
            _ => throw new ArgumentOutOfRangeException(nameof(chargeType), chargeType, null)
        };

    private static IDistribution<Flag> VerticalDibandDist() =>
        from left in AllColoursDist()
        from right in AllColoursExceptDist(left)
        select (Flag)new VerticalDiband(left, right);

    private static IDistribution<Flag> HorizontalDibandDist() =>
        from top in AllColoursDist()
        from bottom in AllColoursExceptDist(top)
        select (Flag)new HorizontalDiband(top, bottom);

    private static IDistribution<Flag> VerticalTribandDist()
    {
        var chargeTypeDist = WeightedDiscreteDistribution.New(
        [
            new Weighting<FlagCharge.Type>(FlagCharge.Type.None, 3),
            new Weighting<FlagCharge.Type>(FlagCharge.Type.Star, 1),
        ]);
        
        return from left in AllColoursDist()
            from middle in AllColoursExceptDist(left)
            from right in AllColoursExceptDist(middle)
            from chargeType in chargeTypeDist
            from charge in ChargeDist(chargeType, disallowedColour: middle)
            select (Flag)new VerticalTriband(left, middle, right, charge);
    }

    private static IDistribution<Flag> HorizontalTribandDist()
    {
        var chargeTypeDist = WeightedDiscreteDistribution.New(
        [
            new Weighting<FlagCharge.Type>(FlagCharge.Type.None, 3),
            new Weighting<FlagCharge.Type>(FlagCharge.Type.StarBand, 1),
        ]);
        
        return from top in AllColoursDist()
            from middle in AllColoursExceptDist(top)
            from bottom in AllColoursExceptDist(middle)
            from chargeType in chargeTypeDist
            from charge in ChargeDist(chargeType, disallowedColour: middle)
            select (Flag)new HorizontalTriband(top, middle, bottom, charge);
    }

    private static IDistribution<Flag> CrossDist() =>
        from background in AllColoursDist()
        from foreground in AllColoursExceptDist(background)
        from crossType in UniformDistribution.Create([CrossType.Regular, CrossType.Nordic])
        select (Flag)new Cross(background, foreground, crossType);

    private static IDistribution<FlagColour> AllColoursDist() =>
        WeightedDiscreteDistribution.New(allColourWeightings);

    private static IDistribution<FlagColour> AllColoursExceptDist(FlagColour exceptColour) =>
        WeightedDiscreteDistribution.New(allColourWeightings.Where(w => w.Value != exceptColour));
}