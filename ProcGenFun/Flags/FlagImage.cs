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

        var color = Color.FromArgb(22, 101, 52);

        svgDocument.Children.Add(new SvgRectangle
        {
            Fill = new SvgColourServer(color),
            X = 0,
            Y = 0,
            Width = 300,
            Height = 200
        });

        return svgDocument;
    }
}
