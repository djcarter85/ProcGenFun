namespace ProcGenFun.Flags.Images;

using System.Drawing;
using ProcGenFun.Flags.Model;
using Svg;
using Svg.Pathing;
using static FlagImageSizing;
using static ProcGenFun.Flags.Model.FlagPattern;
using static ProcGenFun.Flags.Model.HorizontalDibandDecoration;

public static class FlagImage
{
    private const float FlagWidth = 18f * U;
    private const float FlagHeight = 12f * U;

    // It is unusual to use the flag height in a width calculation, but we want the pall to be a specific shape. 
    private static readonly PointF PallConfluence = new(FlagHeight * 0.7f, FlagHeight * 0.5f);
    
    public static SvgDocument CreateSvg(Flag flag, string? className = null)
    {
        var svgDocument = new SvgDocument
        {
            ViewBox = new SvgViewBox(0, 0, FlagWidth, FlagHeight),
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
            HorizontalDiband(var top, var bottom, var decoration) => GetHorizontalDibandFlagElements(top, bottom, decoration),
            VerticalTriband(var left, var middle, var right) => GetVerticalTribandFlagElements(left, middle, right),
            HorizontalTriband(var top, var middle, var bottom, var fimbriation) => GetHorizontalTribandFlagElements(top, middle, bottom, fimbriation),
            DiagonalBicolour(var left, var right, var diagonal) => GetDiagonalBicolourFlagElements(left, right, diagonal),
            Cross(var field, var foreground, var crossType) => GetCrossFlagElements(field, foreground, crossType),
            Saltire(var northSouthField, var eastWestField, var foreground) => GetSaltireFlagElements(northSouthField, eastWestField, foreground),
            Quartered(var topLeft, var topRight, var bottomRight, var bottomLeft) => GetQuarteredFlagElements(topLeft, topRight, bottomRight, bottomLeft),
            HorizontalStriped(var colour1, var colour2, var stripeCount) => GetHorizontalStripedFlagElements(colour1, colour2, stripeCount),
            Pall(var field, var foreground) => GetPallFlagElements(field, foreground),
            Rays(var field, var middle, var foreground) => GetRaysFlagElements(field, middle, foreground),
        };

    private static IEnumerable<SvgElement> GetSolidFlagElements(FlagColour field)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(field)),
            X = 0,
            Y = 0,
            Width = FlagWidth,
            Height = FlagHeight
        };
    }

    private static IEnumerable<SvgElement> GetCantonFlagElements(FlagColour field, FlagColour cantonColour)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(field)),
            X = 0,
            Y = 0,
            Width = FlagWidth,
            Height = FlagHeight
        };
        
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(cantonColour)),
            X = 0,
            Y = 0,
            Width = FlagWidth / 2,
            Height = FlagHeight / 2
        };
    }

    private static IEnumerable<SvgRectangle> GetVerticalDibandFlagElements(FlagColour left, FlagColour right)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(left)),
            X = 0,
            Y = 0,
            Width = FlagWidth / 2,
            Height = FlagHeight
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(right)),
            X = FlagWidth / 2,
            Y = 0,
            Width = FlagWidth / 2,
            Height = FlagHeight
        };
    }

    private static IEnumerable<SvgElement> GetHorizontalDibandFlagElements(
        FlagColour top, FlagColour bottom, HorizontalDibandDecoration decoration)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(top)),
            X = 0,
            Y = 0,
            Width = FlagWidth,
            Height = FlagHeight / 2
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(bottom)),
            X = 0,
            Y = FlagHeight / 2,
            Width = FlagWidth,
            Height = FlagHeight / 2
        };

        foreach (var decorationElement in GetHorizontalDibandDecorationElements(decoration))
        {
            yield return decorationElement;
        }
    }

    private static IEnumerable<SvgElement>
        GetHorizontalDibandDecorationElements(HorizontalDibandDecoration decoration) =>
        decoration switch
        {
            None => [],
            Fimbriation(var colour) =>
            [
                new SvgLine
                {
                    Stroke = new SvgColourServer(FlagImageColours.GetColor(colour)),
                    StrokeWidth = 0.75f * U,
                    StartX = 0,
                    StartY = FlagHeight / 2,
                    EndX = FlagWidth,
                    EndY = FlagHeight / 2,
                }
            ],
            Pile(var colour) =>
            [
                new SvgPath
                {
                    Fill = new SvgColourServer(FlagImageColours.GetColor(colour)),
                    PathData =
                    [
                        new SvgMoveToSegment(false, new PointF(0, 0)),
                        new SvgLineSegment(false, PallConfluence),
                        new SvgLineSegment(false, new PointF(0, FlagHeight)),
                        new SvgClosePathSegment(false)
                    ]
                }
            ],
            VerticalBand(var colour) =>
            [
                new SvgRectangle
                {
                    Fill = new SvgColourServer(FlagImageColours.GetColor(colour)),
                    X = 0,
                    Y = 0,
                    Width = 6 * U,
                    Height = FlagHeight
                }
            ]
        };

    private static IEnumerable<SvgElement> GetVerticalTribandFlagElements(FlagColour left, FlagColour middle, FlagColour right)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(left)),
            X = 0,
            Y = 0,
            Width = FlagWidth / 3,
            Height = FlagHeight
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(middle)),
            X = FlagWidth / 3,
            Y = 0,
            Width = FlagWidth / 3,
            Height = FlagHeight
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(right)),
            X = FlagWidth * 2 / 3,
            Y = 0,
            Width = FlagWidth / 3,
            Height = FlagHeight
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
            Width = FlagWidth,
            Height = FlagHeight / 3
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(middle)),
            X = 0,
            Y = FlagHeight / 3,
            Width = FlagWidth,
            Height = FlagHeight / 3
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(bottom)),
            X = 0,
            Y = FlagHeight * 2 / 3,
            Width = FlagWidth,
            Height = FlagHeight / 3
        };

        if (fimbriation != null)
        {
            yield return new SvgLine
            {
                Stroke = new SvgColourServer(FlagImageColours.GetColor(fimbriation.Value)),
                StrokeWidth = 0.75f * U,
                StartX = 0,
                StartY = FlagHeight / 3,
                EndX = FlagWidth,
                EndY = FlagHeight / 3,
            };
            yield return new SvgLine
            {
                Stroke = new SvgColourServer(FlagImageColours.GetColor(fimbriation.Value)),
                StrokeWidth = 0.75f * U,
                StartX = 0,
                StartY = FlagHeight * 2 / 3,
                EndX = FlagWidth,
                EndY = FlagHeight * 2 / 3,
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
                new SvgLineSegment(false, new PointF(0, FlagHeight)),
                new SvgLineSegment(
                    false,
                    diagonal switch
                    {
                        Diagonal.Down => new PointF(FlagWidth, FlagHeight),
                        Diagonal.Up => new PointF(FlagWidth, 0),
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
                new SvgMoveToSegment(false, new PointF(FlagWidth, 0)),
                new SvgLineSegment(false, new PointF(FlagWidth, FlagHeight)),
                new SvgLineSegment(
                    false,
                    diagonal switch
                    {
                        Diagonal.Down => new PointF(0, 0),
                        Diagonal.Up => new PointF(0, FlagHeight),
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
            Width = FlagWidth,
            Height = FlagHeight
        };

        var verticalBarCentre = crossType switch
        {
            CrossType.Regular => FlagWidth / 2,
            CrossType.Nordic => FlagWidth / 3,
            _ => throw new ArgumentOutOfRangeException(nameof(crossType), crossType, null)
        };

        yield return new SvgPath
        {
            PathData = new SvgPathSegment[]
            {
                new SvgMoveToSegment(false, new PointF(verticalBarCentre, 0)),
                new SvgLineSegment(true, new PointF(0, FlagHeight)),
                new SvgMoveToSegment(false, new PointF(0, FlagHeight / 2)),
                new SvgLineSegment(true, new PointF(FlagWidth, 0)),
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
                new SvgLineSegment(false, new PointF(FlagWidth, 0)),
                new SvgLineSegment(false, new PointF(0, FlagHeight)),
                new SvgLineSegment(false, new PointF(FlagWidth, FlagHeight)),
                new SvgClosePathSegment(false)
            }.ToPathData(),
            Fill = new SvgColourServer(FlagImageColours.GetColor(northSouthField)),
        };
        
        yield return new SvgPath
        {
            PathData = new SvgPathSegment[]
            {
                new SvgMoveToSegment(false, new PointF(0, 0)),
                new SvgLineSegment(false, new PointF(0, FlagHeight)),
                new SvgLineSegment(false, new PointF(FlagWidth, 0)),
                new SvgLineSegment(false, new PointF(FlagWidth, FlagHeight)),
                new SvgClosePathSegment(false)
            }.ToPathData(),
            Fill = new SvgColourServer(FlagImageColours.GetColor(eastWestField)),
        };
        
        yield return new SvgPath
        {
            PathData = new SvgPathSegment[]
            {
                new SvgMoveToSegment(false, new PointF(0, 0)),
                new SvgLineSegment(false, new PointF(FlagWidth, FlagHeight)),
                new SvgMoveToSegment(false, new PointF(0, FlagHeight)),
                new SvgLineSegment(false, new PointF(FlagWidth, 0)),
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
            Width = FlagWidth / 2,
            Height = FlagHeight / 2
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(topRight)),
            X = FlagWidth / 2,
            Y = 0,
            Width = FlagWidth / 2,
            Height = FlagHeight / 2
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(bottomRight)),
            X = FlagWidth / 2,
            Y = FlagHeight / 2,
            Width = FlagWidth / 2,
            Height = FlagHeight / 2
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(bottomLeft)),
            X = 0,
            Y = FlagHeight / 2,
            Width = FlagWidth / 2,
            Height = FlagHeight / 2
        };
    }

    private static IEnumerable<SvgElement> GetHorizontalStripedFlagElements(
        FlagColour colour1, FlagColour colour2, int stripeCount)
    {
        var stripeHeight = FlagHeight / stripeCount;

        for (var i = 0; i < stripeCount; i++)
        {
            var colour = i % 2 == 0 ? colour1 : colour2;

            yield return new SvgRectangle
            {
                Fill = new SvgColourServer(FlagImageColours.GetColor(colour)),
                X = 0,
                Y = stripeHeight * i,
                Width = FlagWidth,
                Height = stripeHeight
            };
        }
    }

    private static IEnumerable<SvgElement> GetPallFlagElements(FlagColour field, FlagColour foreground)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(field)),
            X = 0,
            Y = 0,
            Width = FlagWidth,
            Height = FlagHeight
        };
        
        yield return GetPallElement(foreground, strokeWidth: 2 * U);
    }

    private static SvgPath GetPallElement(FlagColour colour, SvgUnit strokeWidth) =>
        new()
        {
            Stroke = new SvgColourServer(FlagImageColours.GetColor(colour)),
            StrokeWidth = strokeWidth,
            Fill = new  SvgColourServer(Color.Transparent),
            PathData =
            [
                new SvgMoveToSegment(false, new PointF(0, 0)),
                new SvgLineSegment(false, PallConfluence),
                new SvgLineSegment(false, new PointF(FlagWidth, FlagHeight / 2)),
                new SvgMoveToSegment(false, new PointF(0, FlagHeight)),
                new SvgLineSegment(false, PallConfluence),
            ]
        };

    private static IEnumerable<SvgElement> GetRaysFlagElements(
        FlagColour field, FlagColour middle, FlagColour foreground)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(field)),
            X = 0,
            Y = 0,
            Width = FlagWidth,
            Height = FlagHeight
        };
        yield return new SvgPath
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(middle)),
            PathData =
            [
                new SvgMoveToSegment(false, new PointF(0, 0)),
                new SvgLineSegment(false, new PointF(FlagWidth, 0)),
                new SvgLineSegment(false, new PointF(0, FlagHeight)),
                new SvgClosePathSegment(false)
            ]
        };
        yield return new SvgPath
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(foreground)),
            PathData =
            [
                new SvgMoveToSegment(false, new PointF(0, 0)),
                new SvgLineSegment(false, new PointF(FlagWidth / 2, 0)),
                new SvgLineSegment(false, new PointF(0, FlagHeight)),
                new SvgClosePathSegment(false)
            ]
        };
    }
}
