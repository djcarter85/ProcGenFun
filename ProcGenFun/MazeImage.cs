namespace ProcGenFun;

using Svg;
using Svg.Pathing;
using System.Drawing;

public static class MazeImage
{
    private const int marginX = 25;
    private const int marginY = 25;

    private const int cellWidth = 25;
    private const int cellHeight = 25;

    private static readonly Color cellColor = Theme.White;
    private static readonly Color wallColor = Theme.Blue900;

    public static SvgDocument CreateSvg(Maze maze)
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

        svgDocument.Children.Add(
            new SvgRectangle
            {
                Fill = new SvgColourServer(Color.FromArgb(135, 197, 232)),
                X = Left(new Cell(4, 0)),
                Y = Top(new Cell(4, 0)),
                Width = cellWidth,
                Height = maze.Grid.Height * cellHeight
            });

        svgDocument.Children.Add(
            new SvgRectangle
            {
                Fill = new SvgColourServer(Color.FromArgb(135, 197, 232)),
                X = Left(new Cell(0, 6)),
                Y = Top(new Cell(0, 6)),
                Width = maze.Grid.Width * cellWidth,
                Height = cellHeight
            });

        svgDocument.Children.Add(
            new SvgRectangle
            {
                Fill = new SvgColourServer(Color.FromArgb(22, 110, 162)),
                X = Left(new Cell(4, 6)),
                Y = Top(new Cell(4, 6)),
                Width = cellWidth,
                Height = cellHeight
            });

        for (var x = 0; x < maze.Grid.Width; x++)
        {
            svgDocument.Children.Add(
                new SvgText()
                {
                    TextAnchor = SvgTextAnchor.Middle,
                    FontFamily = "sans-serif",
                    Text = x.ToString(),
                    X = [marginX + x * cellWidth + 12],
                    Y = [18]
                });
        }

        for (var y = 0; y < maze.Grid.Height; y++)
        {
            svgDocument.Children.Add(
                new SvgText()
                {
                    TextAnchor = SvgTextAnchor.Middle,
                    FontFamily = "sans-serif",
                    Text = y.ToString(),
                    X = [12],
                    Y = [marginY + y * cellHeight + 18]
                });
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
