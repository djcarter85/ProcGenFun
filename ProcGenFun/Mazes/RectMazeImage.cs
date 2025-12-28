namespace ProcGenFun.Mazes;

using System.Drawing;
using Svg;
using Svg.Pathing;

public static class RectMazeImage
{
    private const int cellWidth = 25;
    private const int cellHeight = 25;

    public static SvgDocument CreateSvg(Maze<RectCell> maze, Func<RectCell, Color> getCellColor, RectGrid grid)
    {
        var imageWidth = ImageWidth(grid);
        var imageHeight = ImageHeight(grid);

        var svgDocument = new SvgDocument
        {
            Width = imageWidth,
            Height = imageHeight,
            ViewBox = new SvgViewBox(0, 0, imageWidth, imageHeight)
        };

        foreach (var cell in grid.Cells)
        {
            svgDocument.Children.Add(DrawCell(cell, color: getCellColor(cell)));
        }

        svgDocument.Children.Add(DrawWalls(maze, grid));

        return svgDocument;
    }

    private static SvgRectangle DrawCell(RectCell cell, Color color) =>
        new SvgRectangle
        {
            Fill = new SvgColourServer(color),
            X = Left(cell),
            Y = Top(cell),
            Width = cellWidth,
            Height = cellHeight
        };

    private static SvgPath DrawWalls(Maze<RectCell> maze, RectGrid grid)
    {
        var pathData = new SvgPathSegmentList();
        foreach (var segment in HorizontalWalls(maze, grid).Concat(VerticalWalls(maze, grid)))
        {
            pathData.Add(segment);
        }

        return new SvgPath
        {
            Stroke = new SvgColourServer(MazeImage.WallColor),
            StrokeWidth = 1,
            PathData = pathData
        };
    }

    private static IEnumerable<SvgPathSegment> HorizontalWalls(Maze<RectCell> maze, RectGrid grid)
    {
        yield return new SvgMoveToSegment(false, TopLeft(new RectCell(0, 0)));

        foreach (var cell in grid.Cells.Where(c => c.Y == 0))
        {
            yield return WallOrBlank(maze.WallExists(grid, cell, RectDirection.North), TopRight(cell));
        }

        foreach (var grouping in grid.Cells.GroupBy(c => c.Y))
        {
            yield return new SvgMoveToSegment(false, BottomLeft(new RectCell(0, grouping.Key)));

            foreach (var cell in grouping)
            {
                yield return WallOrBlank(maze.WallExists(grid, cell, RectDirection.South), BottomRight(cell));
            }
        }
    }

    private static IEnumerable<SvgPathSegment> VerticalWalls(Maze<RectCell> maze, RectGrid grid)
    {
        yield return new SvgMoveToSegment(false, TopLeft(new RectCell(0, 0)));

        foreach (var cell in grid.Cells.Where(c => c.X == 0))
        {
            yield return WallOrBlank(maze.WallExists(grid, cell, RectDirection.West), BottomLeft(cell));
        }

        foreach (var grouping in grid.Cells.GroupBy(c => c.X))
        {
            yield return new SvgMoveToSegment(false, TopRight(new RectCell(grouping.Key, 0)));

            foreach (var cell in grouping)
            {
                yield return WallOrBlank(maze.WallExists(grid, cell, RectDirection.East), BottomRight(cell));
            }
        }
    }

    private static SvgPathSegment WallOrBlank(bool wallExists, Point endpoint) =>
        wallExists ? new SvgLineSegment(false, endpoint) : new SvgMoveToSegment(false, endpoint);

    private static int ImageHeight(RectGrid grid) => grid.Height * cellHeight + 2 * MazeImage.MarginYInPixels;

    private static int ImageWidth(RectGrid grid) => grid.Width * cellWidth + 2 * MazeImage.MarginXInPixels;

    private static int Left(RectCell cell) => MazeImage.MarginXInPixels + cell.X * cellWidth;

    private static int Right(RectCell cell) => Left(cell) + cellWidth;

    private static int Top(RectCell cell) => MazeImage.MarginYInPixels + cell.Y * cellWidth;

    private static int Bottom(RectCell cell) => Top(cell) + cellWidth;

    private static Point TopLeft(RectCell cell) => new Point(Left(cell), Top(cell));

    private static Point TopRight(RectCell cell) => new Point(Right(cell), Top(cell));

    private static Point BottomLeft(RectCell cell) => new Point(Left(cell), Bottom(cell));

    private static Point BottomRight(RectCell cell) => new Point(Right(cell), Bottom(cell));
}
