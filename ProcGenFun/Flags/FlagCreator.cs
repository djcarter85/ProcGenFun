namespace ProcGenFun.Flags;

using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;
using System;
using System.ComponentModel;
using static Flag;
using static FlagCharge;

public static class FlagCreator
{
    public static IDistribution<Flag> FlagDist() =>
        from flagType in FlagTypeDist()
        from flag in FlagDist(flagType)
        select flag;

    private static IDistribution<Flag.Type> FlagTypeDist() =>
        WeightedDiscreteDistribution.New(
            [
                new Weighting<Flag.Type>(Flag.Type.Solid, 15),
                new Weighting<Flag.Type>(Flag.Type.VerticalDiband, 25),
                new Weighting<Flag.Type>(Flag.Type.HorizontalDiband, 35),
                new Weighting<Flag.Type>(Flag.Type.VerticalTriband, 50),
                new Weighting<Flag.Type>(Flag.Type.HorizontalTriband, 45),
                new Weighting<Flag.Type>(Flag.Type.DiagonalBicolour, 5),
                new Weighting<Flag.Type>(Flag.Type.Cross, 25),
                new Weighting<Flag.Type>(Flag.Type.Saltire, 15),
                new Weighting<Flag.Type>(Flag.Type.Quartered, 10),
                new Weighting<Flag.Type>(Flag.Type.HorizontalStriped, 10),
            ]);

    private static IDistribution<Flag> FlagDist(Flag.Type flagType) =>
        flagType switch
        {
            Flag.Type.Solid => SolidFlagDist(),
            Flag.Type.VerticalDiband => VerticalDibandDist(),
            Flag.Type.HorizontalDiband => HorizontalDibandDist(),
            Flag.Type.VerticalTriband => VerticalTribandDist(),
            Flag.Type.HorizontalTriband => HorizontalTribandDist(),
            Flag.Type.DiagonalBicolour => DiagonalBicolourDist(),
            Flag.Type.Cross => CrossDist(),
            Flag.Type.Saltire => SaltireDist(),
            Flag.Type.Quartered => QuarteredDist(),
            Flag.Type.HorizontalStriped => HorizontalStripedDist(),
            _ => throw new ArgumentOutOfRangeException(nameof(flagType), flagType, null)
        };

    private static IDistribution<Flag> SolidFlagDist()
    {
        var chargeTypeDist = WeightedDiscreteDistribution.New(
        [
            new Weighting<FlagCharge.Type>(FlagCharge.Type.None, 1),
            new Weighting<FlagCharge.Type>(FlagCharge.Type.Star, 2),
            new Weighting<FlagCharge.Type>(FlagCharge.Type.Circle, 2),
        ]);
        
        return from colour in FlagColours.AllDist()
            from chargeType in chargeTypeDist
            from charge in ChargeDist(chargeType, backgroundColour: colour)
            select (Flag)new Solid(colour, charge);
    }

    private static IDistribution<FlagCharge> ChargeDist(FlagCharge.Type chargeType, FlagColour backgroundColour) =>
        chargeType switch
        {
            FlagCharge.Type.None =>
                Singleton.New<FlagCharge>(new None()),
            FlagCharge.Type.Star =>
                from colour in FlagColours.AllowedAdjacentToDist(backgroundColour)
                select (FlagCharge)new Star(colour),
            FlagCharge.Type.StarBand =>
                from colour in FlagColours.AllowedAdjacentToDist(backgroundColour)
                from count in Uniform.NewInclusive(1, 4)
                select (FlagCharge)new StarBand(colour, count),
            FlagCharge.Type.Circle =>
                from colour in FlagColours.AllowedAdjacentToDist(backgroundColour)
                select (FlagCharge)new Circle(colour),
            _ => throw new ArgumentOutOfRangeException(nameof(chargeType), chargeType, null)
        };

    private static IDistribution<Flag> VerticalDibandDist() =>
        from left in FlagColours.AllDist()
        from right in FlagColours.AllowedAdjacentToDist(left)
        select (Flag)new VerticalDiband(left, right);

    private static IDistribution<Flag> HorizontalDibandDist() =>
        from top in FlagColours.AllDist()
        from bottom in FlagColours.AllowedAdjacentToDist(top)
        select (Flag)new HorizontalDiband(top, bottom);

    private static IDistribution<Flag> VerticalTribandDist()
    {
        var chargeTypeDist = WeightedDiscreteDistribution.New(
        [
            new Weighting<FlagCharge.Type>(FlagCharge.Type.None, 3),
            new Weighting<FlagCharge.Type>(FlagCharge.Type.Star, 1),
        ]);
        
        return from left in FlagColours.AllDist()
            from middle in FlagColours.AllowedAdjacentToDist(left)
            from right in FlagColours.AllowedAdjacentToDist(middle)
            from chargeType in chargeTypeDist
            from charge in ChargeDist(chargeType, backgroundColour: middle)
            select (Flag)new VerticalTriband(left, middle, right, charge);
    }

    private static IDistribution<Flag> HorizontalTribandDist()
    {
        var chargeTypeDist = WeightedDiscreteDistribution.New(
        [
            new Weighting<FlagCharge.Type>(FlagCharge.Type.None, 3),
            new Weighting<FlagCharge.Type>(FlagCharge.Type.StarBand, 1),
        ]);
        
        return from top in FlagColours.AllDist()
            from middle in FlagColours.AllowedAdjacentToDist(top)
            from bottom in FlagColours.AllowedAdjacentToDist(middle)
            from chargeType in chargeTypeDist
            from charge in ChargeDist(chargeType, backgroundColour: middle)
            select (Flag)new HorizontalTriband(top, middle, bottom, charge);
    }

    private static IDistribution<Flag> DiagonalBicolourDist() =>
        from left in FlagColours.AllDist()
        from right in FlagColours.AllowedAdjacentToDist(left)
        from diagonal in UniformDistribution.Create([Diagonal.Down, Diagonal.Up])
        select (Flag)new DiagonalBicolour(left, right, diagonal);

    private static IDistribution<Flag> CrossDist() =>
        from background in FlagColours.AllDist()
        from foreground in FlagColours.AllowedAdjacentToDist(background)
        from crossType in UniformDistribution.Create([CrossType.Regular, CrossType.Nordic])
        select (Flag)new Cross(background, foreground, crossType);

    private static IDistribution<Flag> SaltireDist()
    {
        IDistribution<FlagColour> EastWestFieldDist(FlagColour northSouthField, bool fieldColoursAreSame) => 
            fieldColoursAreSame ? Singleton.New(northSouthField) : FlagColours.AllExceptDist(northSouthField);

        return from northSouthField in FlagColours.AllDist()
            from fieldColoursAreSame in Bernoulli.FromRatio(4, 5)
            from eastWestfield in EastWestFieldDist(northSouthField, fieldColoursAreSame)
            from foreground in FlagColours.AllowedAdjacentToDist([northSouthField, eastWestfield])
            select (Flag)new Saltire(northSouthField, eastWestfield, foreground);
    }

    private static IDistribution<Flag> QuarteredDist() =>
        from topLeft in FlagColours.AllDist()
        from topRight in FlagColours.AllowedAdjacentToDist(topLeft)
        from bottomRight in FlagColours.AllowedAdjacentToDist(topRight)
        from bottomLeft in FlagColours.AllowedAdjacentToDist([topLeft, bottomRight])
        select (Flag)new Quartered(topLeft, topRight, bottomRight, bottomLeft);

    private static IDistribution<Flag> HorizontalStripedDist() =>
        from colour1 in FlagColours.AllDist()
        from colour2 in FlagColours.AllowedAdjacentToDist(colour1)
        from stripeCount in UniformDistribution.Create([5, 7, 9, 11, 13])
        select (Flag)new HorizontalStriped(colour1, colour2, stripeCount);
}