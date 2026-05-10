namespace ProcGenFun.Flags.Images;

using System.Drawing;
using ProcGenFun.Flags.Model;
using Svg;
using Svg.Pathing;
using static FlagImageSizing;
using static ProcGenFun.Flags.Model.FlagPattern;
using static ProcGenFun.Flags.Model.HorizontalBisectionDecoration;

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
            Solid solid => GetSolidFlagElements(solid),
            Canton canton => GetCantonFlagElements(canton),
            VerticalBisection verticalBisection => GetVerticalBisectionFlagElements(verticalBisection),
            HorizontalBisection horizontalBisection => GetHorizontalBisectionFlagElements(horizontalBisection),
            VerticalTriband verticalTriband => GetVerticalTribandFlagElements(verticalTriband),
            HorizontalTriband horizontalTriband => GetHorizontalTribandFlagElements(horizontalTriband),
            DiagonalBisection diagonalBisection => GetDiagonalBisectionFlagElements(diagonalBisection),
            DiagonalBand diagonalBand => GetDiagonalBandFlagElements(diagonalBand),
            Cross cross => GetCrossFlagElements(cross),
            Saltire saltire => GetSaltireFlagElements(saltire),
            Quadrisection quadrisection => GetQuadrisectionFlagElements(quadrisection),
            HorizontalStriped horizontalStriped => GetHorizontalStripedFlagElements(horizontalStriped),
            Pall pall => GetPallFlagElements(pall),
        };

    private static IEnumerable<SvgElement> GetSolidFlagElements(Solid solid)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(solid.Field)),
            X = 0,
            Y = 0,
            Width = FlagWidth,
            Height = FlagHeight
        };
    }

    private static IEnumerable<SvgElement> GetCantonFlagElements(Canton canton)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(canton.Field)),
            X = 0,
            Y = 0,
            Width = FlagWidth,
            Height = FlagHeight
        };
        
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(canton.CantonColour)),
            X = 0,
            Y = 0,
            Width = FlagWidth / 2,
            Height = FlagHeight / 2
        };
    }

    private static IEnumerable<SvgRectangle> GetVerticalBisectionFlagElements(VerticalBisection verticalBisection)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(verticalBisection.Left)),
            X = 0,
            Y = 0,
            Width = FlagWidth / 2,
            Height = FlagHeight
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(verticalBisection.Right)),
            X = FlagWidth / 2,
            Y = 0,
            Width = FlagWidth / 2,
            Height = FlagHeight
        };
    }

    private static IEnumerable<SvgElement> GetHorizontalBisectionFlagElements(HorizontalBisection horizontalBisection)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(horizontalBisection.Top)),
            X = 0,
            Y = 0,
            Width = FlagWidth,
            Height = FlagHeight / 2
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(horizontalBisection.Bottom)),
            X = 0,
            Y = FlagHeight / 2,
            Width = FlagWidth,
            Height = FlagHeight / 2
        };

        foreach (var decorationElement in GetHorizontalDibandDecorationElements(horizontalBisection.Decoration))
        {
            yield return decorationElement;
        }
    }

    private static IEnumerable<SvgElement>
        GetHorizontalDibandDecorationElements(HorizontalBisectionDecoration decoration) =>
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

    private static IEnumerable<SvgElement> GetVerticalTribandFlagElements(VerticalTriband verticalTriband)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(verticalTriband.Left)),
            X = 0,
            Y = 0,
            Width = FlagWidth / 3,
            Height = FlagHeight
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(verticalTriband.Middle)),
            X = FlagWidth / 3,
            Y = 0,
            Width = FlagWidth / 3,
            Height = FlagHeight
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(verticalTriband.Right)),
            X = FlagWidth * 2 / 3,
            Y = 0,
            Width = FlagWidth / 3,
            Height = FlagHeight
        };
    }

    private static IEnumerable<SvgElement> GetHorizontalTribandFlagElements(HorizontalTriband horizontalTriband)
    {
        var upperBandDivide = horizontalTriband.Sizing switch
        {
            HorizontalTribandSizing.Equal => FlagHeight / 3,
            HorizontalTribandSizing.LargeMiddle => 0.26f * FlagHeight,
            HorizontalTribandSizing.SmallMiddle => 0.4f * FlagHeight,
            HorizontalTribandSizing.LargeTop => 0.5f * FlagHeight,
            _ => throw new ArgumentOutOfRangeException(nameof(horizontalTriband.Sizing), horizontalTriband.Sizing, null)
        };
        var lowerBandDivide = horizontalTriband.Sizing switch
        {
            HorizontalTribandSizing.Equal => FlagHeight * 2 / 3,
            HorizontalTribandSizing.LargeMiddle => 0.74f * FlagHeight,
            HorizontalTribandSizing.SmallMiddle => 0.6f * FlagHeight,
            HorizontalTribandSizing.LargeTop => 0.75f * FlagHeight,
            _ => throw new ArgumentOutOfRangeException(nameof(horizontalTriband.Sizing), horizontalTriband.Sizing, null)
        };
        
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(horizontalTriband.Top)),
            X = 0,
            Y = 0,
            Width = FlagWidth,
            Height = upperBandDivide
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(horizontalTriband.Middle)),
            X = 0,
            Y = upperBandDivide,
            Width = FlagWidth,
            Height = lowerBandDivide - upperBandDivide
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(horizontalTriband.Bottom)),
            X = 0,
            Y = lowerBandDivide,
            Width = FlagWidth,
            Height = FlagHeight - lowerBandDivide
        };

        if (horizontalTriband.Fimbriation != null)
        {
            yield return new SvgLine
            {
                Stroke = new SvgColourServer(FlagImageColours.GetColor(horizontalTriband.Fimbriation.Value)),
                StrokeWidth = 0.75f * U,
                StartX = 0,
                StartY = upperBandDivide,
                EndX = FlagWidth,
                EndY = upperBandDivide,
            };
            yield return new SvgLine
            {
                Stroke = new SvgColourServer(FlagImageColours.GetColor(horizontalTriband.Fimbriation.Value)),
                StrokeWidth = 0.75f * U,
                StartX = 0,
                StartY = lowerBandDivide,
                EndX = FlagWidth,
                EndY = lowerBandDivide,
            };
        }
    }

    private static IEnumerable<SvgElement> GetDiagonalBisectionFlagElements(DiagonalBisection diagonalBisection)
    {
        yield return new SvgPath
        {
            PathData = new SvgPathSegment[]
            {
                new SvgMoveToSegment(false, new PointF(0, 0)),
                new SvgLineSegment(false, new PointF(0, FlagHeight)),
                new SvgLineSegment(
                    false,
                    diagonalBisection.Diagonal switch
                    {
                        Diagonal.Down => new PointF(FlagWidth, FlagHeight),
                        Diagonal.Up => new PointF(FlagWidth, 0),
                        _ => throw new NotImplementedException()
                    }),
                new SvgClosePathSegment(false)
            }.ToPathData(),
            Fill = new SvgColourServer(FlagImageColours.GetColor(diagonalBisection.Left)),
        };
        yield return new SvgPath
        {
            PathData = new SvgPathSegment[]
            {
                new SvgMoveToSegment(false, new PointF(FlagWidth, 0)),
                new SvgLineSegment(false, new PointF(FlagWidth, FlagHeight)),
                new SvgLineSegment(
                    false,
                    diagonalBisection.Diagonal switch
                    {
                        Diagonal.Down => new PointF(0, 0),
                        Diagonal.Up => new PointF(0, FlagHeight),
                        _ => throw new NotImplementedException()
                    }),
                new SvgClosePathSegment(false)
            }.ToPathData(),
            Fill = new SvgColourServer(FlagImageColours.GetColor(diagonalBisection.Right)),
        };

        foreach (var element in GetDiagonalBicolourDecorationElements(diagonalBisection.Decoration, diagonalBisection.Diagonal))
        {
            yield return element;
        }
    }

    private static IEnumerable<SvgElement> GetDiagonalBandFlagElements(DiagonalBand diagonalBand)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(diagonalBand.Field)),
            X = 0,
            Y = 0,
            Width = FlagWidth,
            Height = FlagHeight
        };

        if (diagonalBand.Fimbriation != null)
        {
            yield return DiagonalBandFlagElement(diagonalBand.Fimbriation.Value, strokeWidth: 3.5f * U);
        }
        
        yield return DiagonalBandFlagElement(diagonalBand.Band, strokeWidth: 2.5f * U);

        SvgPath DiagonalBandFlagElement(FlagColour colour, SvgUnit strokeWidth)
        {
            var bandStart = diagonalBand.Diagonal switch {
                Diagonal.Down => new PointF(0, 0),
                Diagonal.Up => new PointF(0, FlagHeight),
                _ => throw new ArgumentOutOfRangeException()
            };
            var bandEnd = diagonalBand.Diagonal switch {
                Diagonal.Down => new PointF(FlagWidth, FlagHeight),
                Diagonal.Up => new PointF(FlagWidth, 0),
                _ => throw new ArgumentOutOfRangeException()
            };

            return new SvgPath
            {
                PathData = new SvgPathSegment[]
                {
                    new SvgMoveToSegment(false,  bandStart),
                    new SvgLineSegment(false, bandEnd),
                }.ToPathData(),
                Stroke = new SvgColourServer(FlagImageColours.GetColor(colour)),
                StrokeWidth = strokeWidth
            };
        }
    }

    private static IEnumerable<SvgElement> GetDiagonalBicolourDecorationElements(
        DiagonalBisectionDecoration decoration, Diagonal diagonal) =>
        decoration switch
        {
            DiagonalBisectionDecoration.None => [],
            DiagonalBisectionDecoration.LeftRay leftRay =>
            [
                new SvgPath
                {
                    PathData =
                    [
                        new SvgMoveToSegment(false, new PointF(0, 0)),
                        new SvgLineSegment(false, new PointF(0, FlagHeight)),
                        new SvgLineSegment(
                            false,
                            new PointF(
                                FlagWidth / 2,
                                diagonal switch
                                {
                                    Diagonal.Down => FlagHeight,
                                    Diagonal.Up => 0,
                                    _ => throw new ArgumentOutOfRangeException(nameof(diagonal), diagonal, null)
                                })),
                        new SvgClosePathSegment(false)
                    ],
                    Fill = new SvgColourServer(FlagImageColours.GetColor(leftRay.Colour)),
                }
            ],
            DiagonalBisectionDecoration.RightRay rightRay =>
            [
                new SvgPath
                {
                    PathData =
                    [
                        new SvgMoveToSegment(false, new PointF(FlagWidth, 0)),
                        new SvgLineSegment(false, new PointF(FlagWidth, FlagHeight)),
                        new SvgLineSegment(
                            false,
                            new PointF(
                                FlagWidth / 2,
                                diagonal switch
                                {
                                    Diagonal.Down => 0,
                                    Diagonal.Up => FlagHeight,
                                    _ => throw new ArgumentOutOfRangeException(nameof(diagonal), diagonal, null)
                                })),
                        new SvgClosePathSegment(false)
                    ],
                    Fill = new SvgColourServer(FlagImageColours.GetColor(rightRay.Colour)),
                }
            ],
        };

    private static IEnumerable<SvgElement> GetCrossFlagElements(Cross cross)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(cross.Field)),
            X = 0,
            Y = 0,
            Width = FlagWidth,
            Height = FlagHeight
        };

        var verticalBarCentre = cross.CrossType switch
        {
            CrossType.Regular => FlagWidth / 2,
            CrossType.Nordic => FlagWidth / 3,
            _ => throw new ArgumentOutOfRangeException(nameof(cross.CrossType), cross.CrossType, null)
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
            Stroke = new SvgColourServer(FlagImageColours.GetColor(cross.Foreground)),
            StrokeWidth = 2.5f * U
        };
    }

    private static IEnumerable<SvgElement> GetSaltireFlagElements(Saltire saltire)
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
            Fill = new SvgColourServer(FlagImageColours.GetColor(saltire.NorthSouthField)),
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
            Fill = new SvgColourServer(FlagImageColours.GetColor(saltire.EastWestField)),
        };

        if (saltire.Fimbriation.HasValue)
        {
            yield return GetSaltireCrossElement(saltire.Fimbriation.Value, strokeWidth: 3.5f * U);
        }
        
        yield return GetSaltireCrossElement(saltire.Foreground, strokeWidth: 2.5f * U);
    }

    private static SvgPath GetSaltireCrossElement(FlagColour stroke, SvgUnit strokeWidth)
    {
        return new SvgPath
        {
            PathData = new SvgPathSegment[]
            {
                new SvgMoveToSegment(false, new PointF(0, 0)),
                new SvgLineSegment(false, new PointF(FlagWidth, FlagHeight)),
                new SvgMoveToSegment(false, new PointF(0, FlagHeight)),
                new SvgLineSegment(false, new PointF(FlagWidth, 0)),
            }.ToPathData(),
            Stroke = new SvgColourServer(FlagImageColours.GetColor(stroke)),
            StrokeWidth = strokeWidth
        };
    }

    private static IEnumerable<SvgElement> GetQuadrisectionFlagElements(Quadrisection quadrisection)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(quadrisection.TopLeft)),
            X = 0,
            Y = 0,
            Width = FlagWidth / 2,
            Height = FlagHeight / 2
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(quadrisection.TopRight)),
            X = FlagWidth / 2,
            Y = 0,
            Width = FlagWidth / 2,
            Height = FlagHeight / 2
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(quadrisection.BottomRight)),
            X = FlagWidth / 2,
            Y = FlagHeight / 2,
            Width = FlagWidth / 2,
            Height = FlagHeight / 2
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(quadrisection.BottomLeft)),
            X = 0,
            Y = FlagHeight / 2,
            Width = FlagWidth / 2,
            Height = FlagHeight / 2
        };
    }

    private static IEnumerable<SvgElement> GetHorizontalStripedFlagElements(HorizontalStriped horizontalStriped)
    {
        var stripeHeight = FlagHeight / horizontalStriped.StripeCount;

        for (var i = 0; i < horizontalStriped.StripeCount; i++)
        {
            var colour = i % 2 == 0 ? horizontalStriped.Colour1 : horizontalStriped.Colour2;

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

    private static IEnumerable<SvgElement> GetPallFlagElements(Pall pall)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(pall.Field)),
            X = 0,
            Y = 0,
            Width = FlagWidth,
            Height = FlagHeight
        };

        if (pall.Fimbriation.HasValue)
        {
            yield return GetPallElement(pall.Fimbriation.Value, strokeWidth: 3 * U);
        }
        
        yield return GetPallElement(pall.Foreground, strokeWidth: 2 * U);
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
}
