namespace ProcGenFun.Flags;

using Svg;
using System.Drawing;
using Svg.Pathing;
using Svg.Transforms;
using static Flag;
using static FlagCharge;

public static class FlagImage
{
    private const int U = 10;

    public static SvgDocument CreateSvg(Flag flag, string? className = null)
    {
        var imageWidth = 18 * U;
        var imageHeight = 12 * U;

        var svgDocument = new SvgDocument
        {
            ViewBox = new SvgViewBox(0, 0, imageWidth, imageHeight),
            CustomAttributes = { {"class", className} }
        };

        foreach (var child in GetFlagElements(flag))
        {
            svgDocument.Children.Add(child);
        }

        return svgDocument;
    }

    private static IEnumerable<SvgElement> GetFlagElements(Flag flag) =>
        flag switch
        {
            Solid(var colour, var charge) => GetSolidFlagElements(colour, charge),
            VerticalDiband(var left, var right) => GetVerticalDibandFlagElements(left, right),
            HorizontalDiband(var top, var bottom) => GetHorizontalDibandFlagElements(top, bottom),
            VerticalTriband(var left, var middle, var right, var charge) =>
                GetVerticalTribandFlagElements(left, middle, right, charge),
            HorizontalTriband(var top, var middle, var bottom, var charge) => 
                GetHorizontalTribandFlagElements(top, middle, bottom, charge),
            Cross(var background, var foreground, var crossType) => GetCrossFlagElements(background, foreground, crossType),
            Saltire(var background, var foreground) => GetSaltireFlagElements(background, foreground),
        };

    private static IEnumerable<SvgElement> GetSolidFlagElements(FlagColour colour, FlagCharge charge)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(colour)),
            X = 0,
            Y = 0,
            Width = 18 * U,
            Height = 12 * U
        };

        foreach (var chargeElement in GetChargeElements(charge, radius: 3 * U))
        {
            yield return chargeElement;
        }
    }

    private static IEnumerable<SvgElement> GetChargeElements(FlagCharge charge, float radius) =>
        charge switch
        {
            None => [],
            Star(var colour) => GetStarElements(colour, radius),
            StarBand(var colour, var count) => GetStarBandElements(colour, count, radius),
            Circle(var colour) => GetCircleElements(colour, radius),
        };

    private static IEnumerable<SvgElement> GetStarElements(FlagColour colour, float radius)
    {
        yield return CreateSvgStar(
            centre: new PointF(9 * U, 6 * U),
            radius: radius, 
            fillColour: GetColor(colour));
    }

    private static IEnumerable<SvgElement> GetStarBandElements(FlagColour colour, int count, float radius)
    {
        var distanceBetweenCentres = 2.5f * radius;
        var firstCentreX = 9 * U - (count - 1) / 2f * distanceBetweenCentres;
        for (int i = 0; i < count; i++)
        {
            yield return CreateSvgStar(
                centre: new PointF(firstCentreX + i * distanceBetweenCentres, 6 * U),
                radius: radius,
                fillColour: GetColor(colour));
        }
    }

    private static IEnumerable<SvgElement> GetCircleElements(FlagColour colour, float radius)
    {
        yield return new SvgCircle
            { CenterX = 9 * U, CenterY = 6 * U, Radius = radius, Fill = new SvgColourServer(GetColor(colour)) };
    }

    private static SvgPath CreateSvgStar(PointF centre, float radius, Color fillColour) =>
        new()
        {
            PathData = ClosedPath([
                RadialPoint(radius, -0.5f * MathF.PI),
                RadialPoint(radius, 0.3f * MathF.PI),
                RadialPoint(radius, -0.9f * MathF.PI),
                RadialPoint(radius, -0.1f * MathF.PI),
                RadialPoint(radius, 0.7f * MathF.PI),
            ]).ToPathData(),
            Fill = new SvgColourServer(fillColour),
            Transforms =
            [
                new SvgTranslate(centre.X, centre.Y)
            ]
        };

    private static PointF RadialPoint(float radius, float angle) => 
        new(x: radius * MathF.Cos(angle), y: radius * MathF.Sin(angle));

    private static IEnumerable<SvgPathSegment> ClosedPath(IEnumerable<PointF> points)
    {
        var isFirst = true;
        foreach (var point in points)
        {
            if (isFirst)
            {
                yield return new SvgMoveToSegment(false, point);
            }
            else
            {
                yield return new SvgLineSegment(false, point);
            }
            
            isFirst = false;
        }
    }

    private static IEnumerable<SvgRectangle> GetVerticalDibandFlagElements(FlagColour left, FlagColour right)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(left)),
            X = 0,
            Y = 0,
            Width = 9 * U,
            Height = 12 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(right)),
            X = 9 * U,
            Y = 0,
            Width = 9 * U,
            Height = 12 * U
        };
    }

    private static IEnumerable<SvgRectangle> GetHorizontalDibandFlagElements(FlagColour top, FlagColour bottom)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(top)),
            X = 0,
            Y = 0,
            Width = 18 * U,
            Height = 100
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(bottom)),
            X = 0,
            Y = 6 * U,
            Width = 18 * U,
            Height = 6 * U
        };
    }

    private static IEnumerable<SvgElement> GetVerticalTribandFlagElements(FlagColour left, FlagColour middle, FlagColour right, FlagCharge charge)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(left)),
            X = 0,
            Y = 0,
            Width = 6 * U,
            Height = 12 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(middle)),
            X = 6 * U,
            Y = 0,
            Width = 6 * U,
            Height = 12 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(right)),
            X = 12 * U,
            Y = 0,
            Width = 6 * U,
            Height = 12 * U
        };

        foreach (var chargeElement in GetChargeElements(charge, radius: 2 * U))
        {
            yield return chargeElement;
        }
    }

    private static IEnumerable<SvgElement> GetHorizontalTribandFlagElements(FlagColour top, FlagColour middle, FlagColour bottom, FlagCharge charge)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(top)),
            X = 0,
            Y = 0,
            Width = 18 * U,
            Height = 4 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(middle)),
            X = 0,
            Y = 4 * U,
            Width = 18 * U,
            Height = 4 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(bottom)),
            X = 0,
            Y = 8 * U,
            Width = 18 * U,
            Height = 4 * U
        };

        foreach (var chargeElement in GetChargeElements(charge, radius: 1.5f * U))
        {
            yield return chargeElement;
        }
    }

    private static IEnumerable<SvgRectangle> GetCrossFlagElements(
        FlagColour background, FlagColour foreground, CrossType crossType)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(background)),
            X = 0,
            Y = 0,
            Width = 18 * U,
            Height = 12 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(foreground)),
            X = 0,
            Y = 5 * U,
            Width = 18 * U,
            Height = 2 * U
        };

        var verticalBarLeft = crossType switch
        {
            CrossType.Regular => 8,
            CrossType.Nordic => 5,
            _ => throw new ArgumentOutOfRangeException(nameof(crossType), crossType, null)
        };

        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(foreground)),
            X = verticalBarLeft * U,
            Y = 0,
            Width = 2 * U,
            Height = 12 * U
        };
    }

    private static IEnumerable<SvgElement> GetSaltireFlagElements(
        FlagColour background, FlagColour foreground)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(background)),
            X = 0,
            Y = 0,
            Width = 18 * U,
            Height = 12 * U
        };
        yield return new SvgPath
        {
            PathData = new SvgPathSegment[]
            {
                new SvgMoveToSegment(false, new PointF(0, 0)),
                new SvgLineSegment(false, new PointF(18 * U, 12 * U)),
                new SvgMoveToSegment(false, new PointF(0, 12 * U)),
                new SvgLineSegment(false, new PointF(18 * U, 0)),
            }.ToPathData(),
            Stroke = new SvgColourServer(GetColor(foreground)),
            StrokeWidth = 2.5f * U
        };
    }

    private static Color GetColor(FlagColour colour) =>
        colour switch
        {
            FlagColour.Red => Color.FromArgb(206, 17, 38),
            FlagColour.Orange => Color.FromArgb(255, 104, 32),
            FlagColour.Yellow => Color.FromArgb(255, 215, 0),
            FlagColour.Green => Color.FromArgb(0, 102, 51),
            FlagColour.LightBlue => Color.FromArgb(0, 158, 219),
            FlagColour.DarkBlue => Color.FromArgb(0, 56, 168),
            FlagColour.Burgundy => Color.FromArgb(97, 39, 50),
            FlagColour.Purple => Color.FromArgb(158, 55, 122),
            FlagColour.Grey => Color.FromArgb(150, 150, 150),
            FlagColour.White => Color.White,
            FlagColour.Black => Color.Black,
            _ => throw new ArgumentOutOfRangeException(nameof(colour), colour, null)
        };
}
