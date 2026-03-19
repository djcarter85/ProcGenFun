namespace ProcGenFun.Maps;

using System.Drawing;
using Svg;
using Svg.Pathing;
using Svg.Transforms;

public static class MapImage
{
    private const int CellSize = 25;
    private const int Margin = 15;
    private const int VectorLength = 10;
    private const float VectorWidth = 1.5f;
    private const int ArrowHeadSize = 3;
    private const int PointRadius = 2;

    public static SvgDocument Create(Map map)
    {
        var imageWidth = 2 * Margin + CellSize * map.GridPointVectors.Keys.Max(gp => gp.X);
        var imageHeight = 2 * Margin + CellSize * map.GridPointVectors.Keys.Max(gp => gp.Y);

        var svgDocument = new SvgDocument
        {
            ViewBox = new SvgViewBox(0, 0, imageWidth, imageHeight)
        };

        svgDocument.Children.Add(new SvgRectangle
        {
            X = 0,
            Y = 0,
            Width = imageWidth,
            Height = imageHeight,
            Fill = new SvgColourServer(Theme.White),
        });

        foreach (var (gridPoint, vector) in map.GridPointVectors)
        {
            svgDocument.Children.Add(SvgArrow(gridPoint, vector));
        }

        foreach (var gridPoint in map.GridPointVectors.Keys)
        {
            svgDocument.Children.Add(SvgPoint(gridPoint));
        }

        return svgDocument;
    }

    private static SvgPath SvgArrow(GridPoint gridPoint, Vector2 vector)
    {
        var pointX = Margin + gridPoint.X * CellSize;
        var pointY = Margin + gridPoint.Y * CellSize;

        return new SvgPath
        {
            PathData =
            [
                new SvgMoveToSegment(false, new PointF(pointX, pointY)),
                new SvgLineSegment(true, new PointF(VectorLength, 0)),
                new SvgLineSegment(true, new PointF(-ArrowHeadSize, -ArrowHeadSize)),
                new SvgMoveToSegment(true, new PointF(0, 2 * ArrowHeadSize)),
                new SvgLineSegment(true, new PointF(ArrowHeadSize, -ArrowHeadSize)),
            ],
            Stroke = new SvgColourServer(Theme.Blue500),
            StrokeWidth = VectorWidth,
            Fill = new SvgColourServer(Color.Transparent),
            Transforms = [new SvgRotate(RadiansToDegrees(vector.ThetaRadians), pointX, pointY)]
        };
    }

    private static SvgCircle SvgPoint(GridPoint gridPoint)
    {
        return new SvgCircle
        {
            CenterX = Margin + gridPoint.X * CellSize,
            CenterY = Margin + gridPoint.Y * CellSize,
            Radius = PointRadius,
            Fill = new SvgColourServer(Theme.Blue900)
        };
    }

    private static float RadiansToDegrees(float thetaRadians) => thetaRadians * 180 / MathF.PI;
}