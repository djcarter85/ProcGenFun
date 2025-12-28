namespace ProcGenFun.Mazes;

using System.Drawing;
using Svg;
using Svg.Pathing;

public static class HexMazeImage
{
    private const float outerRadiusInPixels = 16;
    private static readonly float innerRadiusInPixels = 0.5f * SqrtF(3) * outerRadiusInPixels;

    public static SvgDocument CreateSvg(HexGrid grid)
    {
        var imageWidthInPixels = ImageWidthInPixels(grid);
        var imageHeightInPixels = ImageHeightInPixels(grid);

        var svgDocument = new SvgDocument
        {
            Width = imageWidthInPixels,
            Height = imageHeightInPixels,
            // Setting min X/Y to be negative half the width/height puts the origin in the centre.
            ViewBox = new SvgViewBox(
                minX: -0.5f * imageWidthInPixels,
                minY: -0.5f * imageHeightInPixels,
                width: imageWidthInPixels,
                height: imageHeightInPixels)
        };

        foreach (var cell in grid.Cells)
        {
            svgDocument.Children.Add(DrawCell(cell, color: Theme.White));
        }

        svgDocument.Children.Add(DrawWalls(grid));

        return svgDocument;
    }

    private static SvgPolygon DrawCell(HexCell cell, Color color)
    {
        var polygon = new SvgPolygon
        {
            Fill = new SvgColourServer(color),
            Points = []
        };

        IEnumerable<PointF> points =
        [
            TopCorner(cell),
            TopRightCorner(cell),
            BottomRightCorner(cell),
            BottomCorner(cell),
            BottomLeftCorner(cell),
            TopLeftCorner(cell),
        ];

        foreach (var point in points)
        {
            polygon.Points.Add(point.X);
            polygon.Points.Add(point.Y);
        }

        return polygon;
    }

    private static float ImageWidthInPixels(HexGrid grid)
    {
        var cellWidthInPixels = 2 * innerRadiusInPixels;
        var numberOfCells = 1 + 2 * grid.MaxDistanceFromOrigin;
        var totalMarginInPixels = 2 * MazeImage.MarginXInPixels;
        return cellWidthInPixels * numberOfCells + totalMarginInPixels;
    }

    private static float ImageHeightInPixels(HexGrid grid)
    {
        // Each cell that is not the origin contributes slightly less than the origin, because hexagonal cells overlap
        // in the Y direction.
        var originCellHeightInPixels = 2 * outerRadiusInPixels;
        var nonOriginCellHeightInPixels = 1.5f * outerRadiusInPixels;
        var numberOfNonOriginCells = 2 * grid.MaxDistanceFromOrigin;
        var totalMarginInPixels = 2 * MazeImage.MarginYInPixels;
        return originCellHeightInPixels + nonOriginCellHeightInPixels * numberOfNonOriginCells + totalMarginInPixels;
    }

    private static SvgPath DrawWalls(HexGrid grid)
    {
        var pathData = new SvgPathSegmentList();
        foreach (var segment in CellWallPathSegments(grid))
        {
            pathData.Add(segment);
        }

        return new SvgPath
        {
            Stroke = new SvgColourServer(MazeImage.WallColor),
            StrokeWidth = 1,
            Fill = SvgPaintServer.None,
            PathData = pathData,
        };
    }

    private static IEnumerable<SvgPathSegment> CellWallPathSegments(HexGrid grid)
    {
        foreach (var cell in grid.Cells)
        {
            yield return new SvgMoveToSegment(false, TopCorner(cell));

            IEnumerable<HexCellWall> hexCellWalls =
            [
                new(HexDirection.NorthEast, ToCorner: TopRightCorner(cell)),
                new(HexDirection.East, ToCorner: BottomRightCorner(cell)),
                new(HexDirection.SouthEast, ToCorner: BottomCorner(cell)),
                new(HexDirection.SouthWest, ToCorner: BottomLeftCorner(cell)),
                new(HexDirection.West, ToCorner: TopLeftCorner(cell)),
                new(HexDirection.NorthWest, ToCorner: TopCorner(cell)),
            ];

            foreach (var cellWall in hexCellWalls)
            {
                if (cellWall.ShouldDraw(grid, cell))
                {
                    yield return new SvgLineSegment(false, cellWall.ToCorner);
                }
                else
                {
                    yield return new SvgMoveToSegment(false, cellWall.ToCorner);
                }
            }
        }
    }

    private static PointF CellCentre(HexCell cell)
    {
        var origin = new PointF(0, 0);

        var qVector = new SizeF(2 * innerRadiusInPixels, 0);
        var rVector = new SizeF(innerRadiusInPixels, 1.5f * outerRadiusInPixels);

        return origin + qVector * cell.Q + rVector * cell.R;
    }

    private static PointF TopCorner(HexCell cell) =>
        CellCentre(cell) + new SizeF(0, -outerRadiusInPixels);

    private static PointF TopRightCorner(HexCell cell) =>
        CellCentre(cell) + new SizeF(innerRadiusInPixels, -0.5f * outerRadiusInPixels);

    private static PointF BottomRightCorner(HexCell cell) =>
        CellCentre(cell) + new SizeF(innerRadiusInPixels, 0.5f * outerRadiusInPixels);

    private static PointF BottomCorner(HexCell cell) =>
        CellCentre(cell) + new SizeF(0, outerRadiusInPixels);

    private static PointF BottomLeftCorner(HexCell cell) =>
        CellCentre(cell) + new SizeF(-innerRadiusInPixels, 0.5f * outerRadiusInPixels);

    private static PointF TopLeftCorner(HexCell cell) =>
        CellCentre(cell) + new SizeF(-innerRadiusInPixels, -0.5f * outerRadiusInPixels);

    private static float SqrtF(double i) => (float)Math.Sqrt(i);

    private record HexCellWall(HexDirection Direction, PointF ToCorner)
    {
        private static readonly IEnumerable<HexDirection> PrimaryDirections =
            [HexDirection.NorthEast, HexDirection.East, HexDirection.SouthEast];

        private bool DirectionIsPrimary => PrimaryDirections.Contains(this.Direction);

        public bool ShouldDraw(HexGrid grid, HexCell cell) =>
            this.DirectionIsPrimary || !grid.IsWithinBounds(cell.GetNeighbourInDirection(this.Direction));
    }
}