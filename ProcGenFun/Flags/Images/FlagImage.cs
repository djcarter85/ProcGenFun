namespace ProcGenFun.Flags.Images;

using System.Drawing;
using ProcGenFun.Flags.Model;
using Svg;
using Svg.Pathing;
using static FlagImageSizing;
using static ProcGenFun.Flags.Model.FlagPattern;

public static class FlagImage
{
    public static SvgDocument CreateSvg(Flag flag, string? className = null)
    {
        var imageWidth = 18 * U;
        var imageHeight = 12 * U;

        var svgDocument = new SvgDocument
        {
            ViewBox = new SvgViewBox(0, 0, imageWidth, imageHeight),
            CustomAttributes = { { "class", className } }
        };

        foreach (var child in GetFlagElements(flag))
        {
            svgDocument.Children.Add(child);
        }

        return svgDocument;
    }

    private static IEnumerable<SvgElement> GetFlagElements(Flag flag) =>
        GetFlagPatternElements(flag.Pattern).Concat(FlagImageCharges.GetChargesElements(flag.Charges));

    private static IEnumerable<SvgElement> GetFlagPatternElements(Model.FlagPattern pattern) =>
        pattern switch
        {
            Solid(var field) => GetSolidFlagElements(field),
            Canton(var field, var cantonColour) => GetCantonFlagElements(field, cantonColour),
            VerticalDiband(var left, var right) => GetVerticalDibandFlagElements(left, right),
            HorizontalDiband(var top, var bottom, var fimbriation) => GetHorizontalDibandFlagElements(top, bottom, fimbriation),
            VerticalTriband(var left, var middle, var right) => GetVerticalTribandFlagElements(left, middle, right),
            HorizontalTriband(var top, var middle, var bottom, var fimbriation) => GetHorizontalTribandFlagElements(top, middle, bottom, fimbriation),
            DiagonalBicolour(var left, var right, var diagonal) => GetDiagonalBicolourFlagElements(left, right, diagonal),
            Cross(var field, var foreground, var crossType) => GetCrossFlagElements(field, foreground, crossType),
            Saltire(var northSouthField, var eastWestField, var foreground) => GetSaltireFlagElements(northSouthField, eastWestField, foreground),
            Quartered(var topLeft, var topRight, var bottomRight, var bottomLeft) => GetQuarteredFlagElements(topLeft, topRight, bottomRight, bottomLeft),
            HorizontalStriped(var colour1, var colour2, var stripeCount) => GetHorizontalStripedFlagElements(colour1, colour2, stripeCount),
        };

    private static IEnumerable<SvgElement> GetSolidFlagElements(FlagColour field)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(field)),
            X = 0,
            Y = 0,
            Width = 18 * U,
            Height = 12 * U
        };
    }

    private static IEnumerable<SvgElement> GetCantonFlagElements(FlagColour field, FlagColour cantonColour)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(field)),
            X = 0,
            Y = 0,
            Width = 18 * U,
            Height = 12 * U
        };
        
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(cantonColour)),
            X = 0,
            Y = 0,
            Width = 9 * U,
            Height = 6 * U
        };
    }

    private static IEnumerable<SvgRectangle> GetVerticalDibandFlagElements(FlagColour left, FlagColour right)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(left)),
            X = 0,
            Y = 0,
            Width = 9 * U,
            Height = 12 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(right)),
            X = 9 * U,
            Y = 0,
            Width = 9 * U,
            Height = 12 * U
        };
    }

    private static IEnumerable<SvgElement> GetHorizontalDibandFlagElements(
        FlagColour top, FlagColour bottom, FlagColour? fimbriation)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(top)),
            X = 0,
            Y = 0,
            Width = 18 * U,
            Height = 6 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(bottom)),
            X = 0,
            Y = 6 * U,
            Width = 18 * U,
            Height = 6 * U
        };

        if (fimbriation != null)
        {
            yield return new SvgLine
            {
                Stroke = new SvgColourServer(FlagImageColours.GetColor(fimbriation.Value)),
                StrokeWidth = 0.75f * U,
                StartX = 0,
                StartY = 6 * U,
                EndX = 18 * U,
                EndY = 6 * U,
            };
        }
    }

    private static IEnumerable<SvgElement> GetVerticalTribandFlagElements(FlagColour left, FlagColour middle, FlagColour right)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(left)),
            X = 0,
            Y = 0,
            Width = 6 * U,
            Height = 12 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(middle)),
            X = 6 * U,
            Y = 0,
            Width = 6 * U,
            Height = 12 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(right)),
            X = 12 * U,
            Y = 0,
            Width = 6 * U,
            Height = 12 * U
        };
    }

    private static IEnumerable<SvgElement> GetHorizontalTribandFlagElements(
        FlagColour top, FlagColour middle, FlagColour bottom, FlagColour? fimbriation)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(top)),
            X = 0,
            Y = 0,
            Width = 18 * U,
            Height = 4 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(middle)),
            X = 0,
            Y = 4 * U,
            Width = 18 * U,
            Height = 4 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(bottom)),
            X = 0,
            Y = 8 * U,
            Width = 18 * U,
            Height = 4 * U
        };

        if (fimbriation != null)
        {
            yield return new SvgLine
            {
                Stroke = new SvgColourServer(FlagImageColours.GetColor(fimbriation.Value)),
                StrokeWidth = 0.75f * U,
                StartX = 0,
                StartY = 4 * U,
                EndX = 18 * U,
                EndY = 4 * U,
            };
            yield return new SvgLine
            {
                Stroke = new SvgColourServer(FlagImageColours.GetColor(fimbriation.Value)),
                StrokeWidth = 0.75f * U,
                StartX = 0,
                StartY = 8 * U,
                EndX = 18 * U,
                EndY = 8 * U,
            };
        }
    }

    private static IEnumerable<SvgElement> GetDiagonalBicolourFlagElements(
        FlagColour left, FlagColour right, Diagonal diagonal)
    {
        yield return new SvgPath
        {
            PathData = new SvgPathSegment[]
            {
                new SvgMoveToSegment(false, new PointF(0, 0)),
                new SvgLineSegment(false, new PointF(0, 12 * U)),
                new SvgLineSegment(
                    false,
                    diagonal switch
                    {
                        Diagonal.Down => new PointF(18 * U, 12 * U),
                        Diagonal.Up => new PointF(18 * U, 0),
                        _ => throw new NotImplementedException()
                    }),
                new SvgClosePathSegment(false)
            }.ToPathData(),
            Fill = new SvgColourServer(FlagImageColours.GetColor(left)),
        };
        yield return new SvgPath
        {
            PathData = new SvgPathSegment[]
            {
                new SvgMoveToSegment(false, new PointF(18 * U, 0)),
                new SvgLineSegment(false, new PointF(18 * U, 12 * U)),
                new SvgLineSegment(
                    false,
                    diagonal switch
                    {
                        Diagonal.Down => new PointF(0, 0),
                        Diagonal.Up => new PointF(0, 12 * U),
                        _ => throw new NotImplementedException()
                    }),
                new SvgClosePathSegment(false)
            }.ToPathData(),
            Fill = new SvgColourServer(FlagImageColours.GetColor(right)),
        };
    }

    private static IEnumerable<SvgElement> GetCrossFlagElements(
        FlagColour field, FlagColour foreground, CrossType crossType)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(field)),
            X = 0,
            Y = 0,
            Width = 18 * U,
            Height = 12 * U
        };

        var verticalBarCentre = crossType switch
        {
            CrossType.Regular => 9,
            CrossType.Nordic => 6,
            _ => throw new ArgumentOutOfRangeException(nameof(crossType), crossType, null)
        };

        yield return new SvgPath
        {
            PathData = new SvgPathSegment[]
            {
                new SvgMoveToSegment(false, new PointF(verticalBarCentre * U, 0)),
                new SvgLineSegment(true, new PointF(0, 12 * U)),
                new SvgMoveToSegment(false, new PointF(0, 6 * U)),
                new SvgLineSegment(true, new PointF(18 * U, 0)),
            }.ToPathData(),
            Stroke = new SvgColourServer(FlagImageColours.GetColor(foreground)),
            StrokeWidth = 2.5f * U
        };
    }

    private static IEnumerable<SvgElement> GetSaltireFlagElements(
        FlagColour northSouthField, FlagColour eastWestField, FlagColour foreground)
    {
        yield return new SvgPath
        {
            PathData = new SvgPathSegment[]
            {
                new SvgMoveToSegment(false, new PointF(0, 0)),
                new SvgLineSegment(false, new PointF(18 * U, 0)),
                new SvgLineSegment(false, new PointF(0, 12 * U)),
                new SvgLineSegment(false, new PointF(18 * U, 12 * U)),
                new SvgClosePathSegment(false)
            }.ToPathData(),
            Fill = new SvgColourServer(FlagImageColours.GetColor(northSouthField)),
        };
        
        yield return new SvgPath
        {
            PathData = new SvgPathSegment[]
            {
                new SvgMoveToSegment(false, new PointF(0, 0)),
                new SvgLineSegment(false, new PointF(0, 12 * U)),
                new SvgLineSegment(false, new PointF(18 * U, 0)),
                new SvgLineSegment(false, new PointF(18 * U, 12 * U)),
                new SvgClosePathSegment(false)
            }.ToPathData(),
            Fill = new SvgColourServer(FlagImageColours.GetColor(eastWestField)),
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
            Stroke = new SvgColourServer(FlagImageColours.GetColor(foreground)),
            StrokeWidth = 2.5f * U
        };
    }

    private static IEnumerable<SvgElement> GetQuarteredFlagElements(
        FlagColour topLeft, FlagColour topRight, FlagColour bottomRight, FlagColour bottomLeft)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(topLeft)),
            X = 0,
            Y = 0,
            Width = 9 * U,
            Height = 6 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(topRight)),
            X = 9 * U,
            Y = 0,
            Width = 9 * U,
            Height = 6 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(bottomRight)),
            X = 9 * U,
            Y = 6 * U,
            Width = 9 * U,
            Height = 6 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(bottomLeft)),
            X = 0,
            Y = 6 * U,
            Width = 9 * U,
            Height = 6 * U
        };
    }

    private static IEnumerable<SvgElement> GetHorizontalStripedFlagElements(
        FlagColour colour1, FlagColour colour2, int stripeCount)
    {
        var stripeHeight = 12f * U / stripeCount;

        for (var i = 0; i < stripeCount; i++)
        {
            var colour = i % 2 == 0 ? colour1 : colour2;

            yield return new SvgRectangle
            {
                Fill = new SvgColourServer(FlagImageColours.GetColor(colour)),
                X = 0,
                Y = stripeHeight * i,
                Width = 18 * U,
                Height = stripeHeight
            };
        }
    }
}
