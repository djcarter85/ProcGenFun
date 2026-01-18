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

    private static Color GetColor(FlagColour colour) =>
        colour switch
        {
            FlagColour.Red => Color.FromArgb(185, 28, 28),
            FlagColour.Orange => Color.FromArgb(249, 115, 22),
            FlagColour.Yellow => Color.FromArgb(250, 204, 21),
            FlagColour.Green => Color.FromArgb(22, 163, 74),
            FlagColour.LightBlue => Color.FromArgb(6, 182, 212),
            FlagColour.DarkBlue => Color.FromArgb(30, 64, 175),
            FlagColour.White => Color.White,
            FlagColour.Black => Color.Black,
            _ => throw new ArgumentOutOfRangeException(nameof(colour), colour, null)
        };
}
