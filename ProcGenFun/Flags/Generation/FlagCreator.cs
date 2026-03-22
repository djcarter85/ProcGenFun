namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Distributions;
using RandN.Extensions;
using static ProcGenFun.Flags.Model.FlagPattern;
using static ProcGenFun.Flags.Model.FlagChargeShape;

public static class FlagCreator
{
    public static IDistribution<Flag> FlagDist() =>
        from flagType in FlagTypeDist()
        from flag in FlagDist(flagType)
        select flag;

    private static IDistribution<FlagPattern.Type> FlagTypeDist() =>
        WeightedDiscreteDistribution.New(
        [
            new Weighting<FlagPattern.Type>(FlagPattern.Type.Solid, 15),
            new Weighting<FlagPattern.Type>(FlagPattern.Type.Canton, 15),
            new Weighting<FlagPattern.Type>(FlagPattern.Type.VerticalDiband, 25),
            new Weighting<FlagPattern.Type>(FlagPattern.Type.HorizontalDiband, 35),
            new Weighting<FlagPattern.Type>(FlagPattern.Type.VerticalTriband, 50),
            new Weighting<FlagPattern.Type>(FlagPattern.Type.HorizontalTriband, 45),
            new Weighting<FlagPattern.Type>(FlagPattern.Type.DiagonalBicolour, 5),
            new Weighting<FlagPattern.Type>(FlagPattern.Type.Cross, 25),
            new Weighting<FlagPattern.Type>(FlagPattern.Type.Saltire, 15),
            new Weighting<FlagPattern.Type>(FlagPattern.Type.Quartered, 10),
            new Weighting<FlagPattern.Type>(FlagPattern.Type.HorizontalStriped, 10),
        ]);

    private static IDistribution<Flag> FlagDist(FlagPattern.Type flagType) =>
        flagType switch
        {
            FlagPattern.Type.Solid => SolidFlagDist(),
            FlagPattern.Type.Canton => CantonFlagDist(),
            FlagPattern.Type.VerticalDiband => VerticalDibandDist(),
            FlagPattern.Type.HorizontalDiband => HorizontalDibandDist(),
            FlagPattern.Type.VerticalTriband => VerticalTribandDist(),
            FlagPattern.Type.HorizontalTriband => HorizontalTribandDist(),
            FlagPattern.Type.DiagonalBicolour => DiagonalBicolourDist(),
            FlagPattern.Type.Cross => CrossDist(),
            FlagPattern.Type.Saltire => SaltireDist(),
            FlagPattern.Type.Quartered => QuarteredDist(),
            FlagPattern.Type.HorizontalStriped => HorizontalStripedDist(),
            _ => throw new ArgumentOutOfRangeException(nameof(flagType), flagType, null)
        };

    private static IDistribution<Flag> SolidFlagDist()
    {
        var chargeTypeDist = WeightedDiscreteDistribution.New(
        [
            new Weighting<FlagChargeShape.Type>(FlagChargeShape.Type.None, 1),
            new Weighting<FlagChargeShape.Type>(FlagChargeShape.Type.Star, 2),
            new Weighting<FlagChargeShape.Type>(FlagChargeShape.Type.Circle, 2),
        ]);

        return from colour in FlagColours.AllDist()
               from chargeType in chargeTypeDist
               from charges in ChargesDist(chargeType, backgroundColour: colour, size: 3)
               select new Flag(new Solid(colour), charges);
    }

    private static IDistribution<Flag> CantonFlagDist() =>
        from field in FlagColours.AllDist()
        from cantonColour in FlagColours.AllowedAdjacentToDist(field)
        select new Flag(new Canton(field, cantonColour), []);

    private static IDistribution<IReadOnlyList<FlagCharge>> ChargesDist(FlagChargeShape.Type chargeType,
        FlagColour backgroundColour, float size) =>
        chargeType switch
        {
            FlagChargeShape.Type.None => Singleton.New<IReadOnlyList<FlagCharge>>([]),
            FlagChargeShape.Type.Star => StarChargeDist(backgroundColour, size),
            FlagChargeShape.Type.StarBand => StarBandChargeDist(backgroundColour, size),
            FlagChargeShape.Type.Circle => CircleChargeDist(backgroundColour, size),
            _ => throw new ArgumentOutOfRangeException(nameof(chargeType), chargeType, null)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>> StarChargeDist(FlagColour backgroundColour, float size) =>
        from colour in FlagColours.AllowedAdjacentToDist(backgroundColour)
        select (IReadOnlyList<FlagCharge>)new List<FlagCharge>
        {
            new(
                new Star(colour),
                size,
                FlagChargeHorizontalLocation.Centre,
                FlagChargeVerticalLocation.Centre)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>>
        StarBandChargeDist(FlagColour backgroundColour, float size) =>
        from colour in FlagColours.AllowedAdjacentToDist(backgroundColour)
        from count in Uniform.NewInclusive(1, 4)
        select (IReadOnlyList<FlagCharge>)new List<FlagCharge>
        {
            new(
                new StarBand(colour, count), 
                size, 
                FlagChargeHorizontalLocation.Centre,
                FlagChargeVerticalLocation.Centre)
        };

    private static IDistribution<IReadOnlyList<FlagCharge>> CircleChargeDist(FlagColour backgroundColour, float size) =>
        from colour in FlagColours.AllowedAdjacentToDist(backgroundColour)
        select (IReadOnlyList<FlagCharge>)new List<FlagCharge>
        {
            new(new Circle(colour),
                size,
                FlagChargeHorizontalLocation.Centre,
                FlagChargeVerticalLocation.Centre)
        };

    private static IDistribution<Flag> VerticalDibandDist()
    {
        var chargeLocationDist = WeightedDiscreteDistribution.New([
            new Weighting<FlagChargeHorizontalLocation?>(null, 4),
            new Weighting<FlagChargeHorizontalLocation?>(FlagChargeHorizontalLocation.Left, 1),
            new Weighting<FlagChargeHorizontalLocation?>(FlagChargeHorizontalLocation.Right, 1),
        ]);
        
        return from left in FlagColours.AllDist()
            from right in FlagColours.AllowedAdjacentToDist(left)
            from chargeLocation in chargeLocationDist
            from charges in VerticalDibandChargesDist(chargeLocation, left, right)
            select new Flag(new VerticalDiband(left, right), charges);

        IDistribution<IReadOnlyList<FlagCharge>> VerticalDibandChargesDist(
            FlagChargeHorizontalLocation? chargeLocation, FlagColour left, FlagColour right) =>
            chargeLocation switch
            {
                FlagChargeHorizontalLocation.Left =>
                    from colour in FlagColours.AllowedAdjacentToDist(left)
                    select (IReadOnlyList<FlagCharge>)
                    [
                        new FlagCharge(new Star(colour),
                            1.5f,
                            FlagChargeHorizontalLocation.Left,
                            FlagChargeVerticalLocation.Centre)
                    ],
                FlagChargeHorizontalLocation.Centre =>
                    throw new ArgumentOutOfRangeException(nameof(chargeLocation), chargeLocation, null),
                FlagChargeHorizontalLocation.Right => 
                    from colour in FlagColours.AllowedAdjacentToDist(right)
                    select (IReadOnlyList<FlagCharge>)
                    [
                        new FlagCharge(new Star(colour),
                            1.5f,
                            FlagChargeHorizontalLocation.Right,
                            FlagChargeVerticalLocation.Centre)
                    ],
                null => Singleton.New<IReadOnlyList<FlagCharge>>([]),
                _ => throw new ArgumentOutOfRangeException(nameof(chargeLocation), chargeLocation, null)
            };
    }

    private static IDistribution<Flag> HorizontalDibandDist() =>
        from top in FlagColours.AllDist()
        from bottom in FlagColours.AllowedAdjacentToDist(top)
        select new Flag(new HorizontalDiband(top, bottom), []);

    private static IDistribution<Flag> VerticalTribandDist()
    {
        var chargeTypeDist = WeightedDiscreteDistribution.New(
        [
            new Weighting<FlagChargeShape.Type>(FlagChargeShape.Type.None, 3),
            new Weighting<FlagChargeShape.Type>(FlagChargeShape.Type.Star, 1),
        ]);

        return from left in FlagColours.AllDist()
               from middle in FlagColours.AllowedAdjacentToDist(left)
               from right in FlagColours.AllowedAdjacentToDist(middle)
               from chargeType in chargeTypeDist
               from charge in ChargesDist(chargeType, backgroundColour: middle, size: 2)
               select new Flag(new VerticalTriband(left, middle, right), charge);
    }

    private static IDistribution<Flag> HorizontalTribandDist()
    {
        var chargeTypeDist = WeightedDiscreteDistribution.New(
        [
            new Weighting<FlagChargeShape.Type>(FlagChargeShape.Type.None, 3),
            new Weighting<FlagChargeShape.Type>(FlagChargeShape.Type.StarBand, 1),
        ]);

        return from top in FlagColours.AllDist()
               from middle in FlagColours.AllowedAdjacentToDist(top)
               from bottom in FlagColours.AllowedAdjacentToDist(middle)
               from chargeType in chargeTypeDist
               from charge in ChargesDist(chargeType, backgroundColour: middle, size: 1.5f)
               select new Flag(new HorizontalTriband(top, middle, bottom), charge);
    }

    private static IDistribution<Flag> DiagonalBicolourDist() =>
        from left in FlagColours.AllDist()
        from right in FlagColours.AllowedAdjacentToDist(left)
        from diagonal in UniformDistribution.Create([Diagonal.Down, Diagonal.Up])
        select new Flag(new DiagonalBicolour(left, right, diagonal), []);

    private static IDistribution<Flag> CrossDist() =>
        from background in FlagColours.AllDist()
        from foreground in FlagColours.AllowedAdjacentToDist(background)
        from crossType in UniformDistribution.Create([CrossType.Regular, CrossType.Nordic])
        select new Flag(new Cross(background, foreground, crossType), []);

    private static IDistribution<Flag> SaltireDist()
    {
        IDistribution<FlagColour> EastWestFieldDist(FlagColour northSouthField, bool fieldColoursAreSame) =>
            fieldColoursAreSame ? Singleton.New(northSouthField) : FlagColours.AllExceptDist(northSouthField);

        return from northSouthField in FlagColours.AllDist()
               from fieldColoursAreSame in Bernoulli.FromRatio(4, 5)
               from eastWestfield in EastWestFieldDist(northSouthField, fieldColoursAreSame)
               from foreground in FlagColours.AllowedAdjacentToDist([northSouthField, eastWestfield])
               select new Flag(new Saltire(northSouthField, eastWestfield, foreground), []);
    }

    private static IDistribution<Flag> QuarteredDist() =>
        from topLeft in FlagColours.AllDist()
        from topRight in FlagColours.AllowedAdjacentToDist(topLeft)
        from bottomRight in FlagColours.AllowedAdjacentToDist(topRight)
        from bottomLeft in FlagColours.AllowedAdjacentToDist([topLeft, bottomRight])
        select new Flag(new Quartered(topLeft, topRight, bottomRight, bottomLeft), []);

    private static IDistribution<Flag> HorizontalStripedDist() =>
        from colour1 in FlagColours.AllDist()
        from colour2 in FlagColours.AllowedAdjacentToDist(colour1)
        from stripeCount in UniformDistribution.Create([5, 7, 9, 11, 13])
        select new Flag(new HorizontalStriped(colour1, colour2, stripeCount), []);
}