namespace ProcGenFun.Flags;

using Svg;
using System.Drawing;

public static class FlagImage
{
    public static SvgDocument CreateSvg(Flag flag)
    {
        var imageWidth = 300;
        var imageHeight = 200;

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

    private static IEnumerable<SvgRectangle> GetFlagElements(Flag flag) =>
        flag switch
        {
            Flag.Solid solid => GetSolidFlagElements(solid),
            Flag.VerticalDiband verticalDiband => GetVerticalDibandFlagElements(verticalDiband),
            Flag.HorizontalDiband horizontalDiband => GetHorizontalDibandFlagElements(horizontalDiband),
            Flag.VerticalTriband verticalTriband => GetVerticalTribandFlagElements(verticalTriband),
            Flag.HorizontalTriband horizontalTriband => GetHorizontalTribandFlagElements(horizontalTriband),
            _ => throw new ArgumentOutOfRangeException(nameof(flag), flag, null),
        };

    private static IEnumerable<SvgRectangle> GetSolidFlagElements(Flag.Solid solid)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(solid.Colour)),
            X = 0,
            Y = 0,
            Width = 300,
            Height = 200
        };
    }

    private static IEnumerable<SvgRectangle> GetVerticalDibandFlagElements(Flag.VerticalDiband verticalDiband)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(verticalDiband.Left)),
            X = 0,
            Y = 0,
            Width = 150,
            Height = 200
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(verticalDiband.Right)),
            X = 150,
            Y = 0,
            Width = 150,
            Height = 200
        };
    }

    private static IEnumerable<SvgRectangle> GetHorizontalDibandFlagElements(Flag.HorizontalDiband horizontalDiband)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(horizontalDiband.Top)),
            X = 0,
            Y = 0,
            Width = 300,
            Height = 100
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(horizontalDiband.Bottom)),
            X = 0,
            Y = 100,
            Width = 300,
            Height = 100
        };
    }

    private static IEnumerable<SvgRectangle> GetVerticalTribandFlagElements(Flag.VerticalTriband verticalTriband)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(verticalTriband.Left)),
            X = 0,
            Y = 0,
            Width = 100,
            Height = 200
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(verticalTriband.Middle)),
            X = 100,
            Y = 0,
            Width = 100,
            Height = 200
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(verticalTriband.Right)),
            X = 200,
            Y = 0,
            Width = 100,
            Height = 200
        };
    }

    private static IEnumerable<SvgRectangle> GetHorizontalTribandFlagElements(Flag.HorizontalTriband verticalTriband)
    {
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(verticalTriband.Top)),
            X = 0,
            Y = 0,
            Width = 300,
            Height = 67
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(verticalTriband.Middle)),
            X = 0,
            Y = 67,
            Width = 300,
            Height = 66
        };
        yield return new SvgRectangle
        {
            Fill = new SvgColourServer(GetColor(verticalTriband.Bottom)),
            X = 0,
            Y = 133,
            Width = 300,
            Height = 67
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
