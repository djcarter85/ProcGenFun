namespace ProcGenFun.Flags;

using Svg;
using System.Drawing;
using Svg.Pathing;
using Svg.Transforms;

public static class FlagImage
{
    private const int U = 10;

    public static SvgDocument CreateSvg(Flag flag)
    {
        var imageWidth = 18 * U;
        var imageHeight = 12 * U;

        var svgDocument = new SvgDocument
        {
            Width = imageWidth,
            Height = imageHeight,
            ViewBox = new SvgViewBox(0, 0, imageWidth, imageHeight)
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
            Flag.Solid solid => GetSolidFlagElements(solid),
            Flag.VerticalDiband verticalDiband => GetVerticalDibandFlagElements(verticalDiband),
            Flag.HorizontalDiband horizontalDiband => GetHorizontalDibandFlagElements(horizontalDiband),
            Flag.VerticalTriband verticalTriband => GetVerticalTribandFlagElements(verticalTriband),
            Flag.HorizontalTriband horizontalTriband => GetHorizontalTribandFlagElements(horizontalTriband),
            Flag.Cross cross => GetCrossFlagElements(cross),
            _ => throw new ArgumentOutOfRangeException(nameof(flag), flag, null),
        };

    private static IEnumerable<SvgElement> GetSolidFlagElements(Flag.Solid solid)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(solid.Colour)),
            X = 0,
            Y = 0,
            Width = 18 * U,
            Height = 12 * U
        };

        foreach (var chargeElement in GetChargeElements(solid.Charge))
        {
            yield return chargeElement;
        }
    }

    private static IEnumerable<SvgElement> GetChargeElements(FlagCharge charge) =>
        charge switch
        {
            FlagCharge.None => [],
            FlagCharge.Star star => GetStarElements(star),
            _ => throw new ArgumentOutOfRangeException(nameof(charge))
        };

    private static IEnumerable<SvgElement> GetStarElements(FlagCharge.Star star)
    {
        yield return new SvgPath
        {
            PathData =
            [
                new SvgMoveToSegment(true, new PointF(0, -3 * U)),
                new SvgLineSegment(true, new PointF(1.7634f * U, 5.427f * U)),
                new SvgLineSegment(true, new PointF(-4.6166f * U, -3.354f * U)),
                new SvgLineSegment(true, new PointF(5.7064f * U, 0)),
                new SvgLineSegment(true, new PointF(-4.6166f * U, 3.354f * U)),
                new SvgClosePathSegment(false)
            ],
            Fill = new SvgColourServer(GetColor(star.Colour)),
            Transforms =
            [
                new SvgTranslate(9 * U, 6 * U)
            ]
        };
    }

    private static IEnumerable<SvgRectangle> GetVerticalDibandFlagElements(Flag.VerticalDiband verticalDiband)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(verticalDiband.Left)),
            X = 0,
            Y = 0,
            Width = 9 * U,
            Height = 12 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(verticalDiband.Right)),
            X = 9 * U,
            Y = 0,
            Width = 9 * U,
            Height = 12 * U
        };
    }

    private static IEnumerable<SvgRectangle> GetHorizontalDibandFlagElements(Flag.HorizontalDiband horizontalDiband)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(horizontalDiband.Top)),
            X = 0,
            Y = 0,
            Width = 18 * U,
            Height = 100
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(horizontalDiband.Bottom)),
            X = 0,
            Y = 6 * U,
            Width = 18 * U,
            Height = 6 * U
        };
    }

    private static IEnumerable<SvgRectangle> GetVerticalTribandFlagElements(Flag.VerticalTriband verticalTriband)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(verticalTriband.Left)),
            X = 0,
            Y = 0,
            Width = 6 * U,
            Height = 12 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(verticalTriband.Middle)),
            X = 6 * U,
            Y = 0,
            Width = 6 * U,
            Height = 12 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(verticalTriband.Right)),
            X = 12 * U,
            Y = 0,
            Width = 6 * U,
            Height = 12 * U
        };
    }

    private static IEnumerable<SvgRectangle> GetHorizontalTribandFlagElements(Flag.HorizontalTriband verticalTriband)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(verticalTriband.Top)),
            X = 0,
            Y = 0,
            Width = 18 * U,
            Height = 4 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(verticalTriband.Middle)),
            X = 0,
            Y = 4 * U,
            Width = 18 * U,
            Height = 4 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(verticalTriband.Bottom)),
            X = 0,
            Y = 8 * U,
            Width = 18 * U,
            Height = 4 * U
        };
    }

    private static IEnumerable<SvgRectangle> GetCrossFlagElements(Flag.Cross cross)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(cross.Background)),
            X = 0,
            Y = 0,
            Width = 18 * U,
            Height = 12 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(cross.Foreground)),
            X = 8 * U,
            Y = 0,
            Width = 2 * U,
            Height = 12 * U
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(cross.Foreground)),
            X = 0,
            Y = 5 * U,
            Width = 18 * U,
            Height = 2 * U
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
            FlagColour.White => Color.White,
            FlagColour.Black => Color.Black,
            _ => throw new ArgumentOutOfRangeException(nameof(colour), colour, null)
        };
}
