namespace ProcGenFun.Mazes;

using System.Drawing;
using Svg;
using Svg.Pathing;

public static class MazeImage
{
    private const int marginX = 25;
    private const int marginY = 25;

    private const int cellWidth = 25;
    private const int cellHeight = 25;

    private static readonly Color cellColor = Theme.White;
    private static readonly Color highlightColor = Theme.Blue300;
    private static readonly Color wallColor = Theme.Blue900;

    public static SvgDocument CreateSvg(Maze maze, IReadOnlyList<Cell>? highlightedCells = null)
    {
        var imageWidth = ImageWidth(maze.Grid);
        var imageHeight = ImageHeight(maze.Grid);

        var svgDocument = new SvgDocument
        {
            Width = imageWidth,
            Height = imageHeight,
            ViewBox = new SvgViewBox(0, 0, imageWidth, imageHeight)
        };

        svgDocument.Children.Add(DrawCells(maze));

        if (highlightedCells != null)
        {
            foreach (var highlightedCell in highlightedCells)
            {
                svgDocument.Children.Add(DrawCell(highlightedCell, color: highlightColor));
            }
        }

        svgDocument.Children.Add(DrawWalls(maze));

        return svgDocument;
    }

    private static SvgRectangle DrawCells(Maze maze) =>
        new SvgRectangle
        {
            Fill = new SvgColourServer(cellColor),
            X = Left(new Cell(0, 0)),
            Y = Top(new Cell(0, 0)),
            Width = maze.Grid.Width * cellWidth,
            Height = maze.Grid.Height * cellHeight
        };

    private static SvgRectangle DrawCell(Cell cell, Color color) =>
        new SvgRectangle
        {
            Fill = new SvgColourServer(color),
            X = Left(cell),
            Y = Top(cell),
            Width = cellWidth,
            Height = cellHeight
        };

    private static SvgPath DrawWalls(Maze maze)
    {
        var pathData = new SvgPathSegmentList();
        foreach (var segment in HorizontalWalls(maze).Concat(VerticalWalls(maze)))
        {
            pathData.Add(segment);
        }

        return new SvgPath
        {
            Stroke = new SvgColourServer(wallColor),
            StrokeWidth = 1,
            PathData = pathData
        };
    }

    private static IEnumerable<SvgPathSegment> HorizontalWalls(Maze maze)
    {
        yield return new SvgMoveToSegment(false, TopLeft(new Cell(0, 0)));

        foreach (var cell in maze.Grid.Cells.Where(c => c.Y == 0))
        {
            yield return WallOrBlank(maze.WallExists(cell, Direction.North), TopRight(cell));
        }

        foreach (var grouping in maze.Grid.Cells.GroupBy(c => c.Y))
        {
            yield return new SvgMoveToSegment(false, BottomLeft(new Cell(0, grouping.Key)));

            foreach (var cell in grouping)
            {
                yield return WallOrBlank(maze.WallExists(cell, Direction.South), BottomRight(cell));
            }
        }
    }

    private static IEnumerable<SvgPathSegment> VerticalWalls(Maze maze)
    {
        yield return new SvgMoveToSegment(false, TopLeft(new Cell(0, 0)));

        foreach (var cell in maze.Grid.Cells.Where(c => c.X == 0))
        {
            yield return WallOrBlank(maze.WallExists(cell, Direction.West), BottomLeft(cell));
        }

        foreach (var grouping in maze.Grid.Cells.GroupBy(c => c.X))
        {
            yield return new SvgMoveToSegment(false, TopRight(new Cell(grouping.Key, 0)));

            foreach (var cell in grouping)
            {
                yield return WallOrBlank(maze.WallExists(cell, Direction.East), BottomRight(cell));
            }
        }
    }

    private static SvgPathSegment WallOrBlank(bool wallExists, Point endpoint) =>
        wallExists ? new SvgLineSegment(false, endpoint) : new SvgMoveToSegment(false, endpoint);

    private static int ImageHeight(Grid grid) => grid.Height * cellHeight + 2 * marginY;

    private static int ImageWidth(Grid grid) => grid.Width * cellWidth + 2 * marginX;

    private static int Left(Cell cell) => marginX + cell.X * cellWidth;

    private static int Right(Cell cell) => Left(cell) + cellWidth;

    private static int Top(Cell cell) => marginY + cell.Y * cellWidth;

    private static int Bottom(Cell cell) => Top(cell) + cellWidth;

    private static Point TopLeft(Cell cell) => new Point(Left(cell), Top(cell));

    private static Point TopRight(Cell cell) => new Point(Right(cell), Top(cell));

    private static Point BottomLeft(Cell cell) => new Point(Left(cell), Bottom(cell));

    private static Point BottomRight(Cell cell) => new Point(Right(cell), Bottom(cell));
}
