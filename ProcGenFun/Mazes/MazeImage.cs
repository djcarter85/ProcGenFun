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

    private static readonly Color wallColor = Theme.Blue900;

    public static SvgDocument CreateSvg(Maze maze, Func<Cell, Color> getCellColor, Grid grid)
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

    private static SvgRectangle DrawCell(Cell cell, Color color) =>
        new SvgRectangle
        {
            Fill = new SvgColourServer(color),
            X = Left(cell),
            Y = Top(cell),
            Width = cellWidth,
            Height = cellHeight
        };

    private static SvgPath DrawWalls(Maze maze, Grid grid)
    {
        var pathData = new SvgPathSegmentList();
        foreach (var segment in HorizontalWalls(maze, grid).Concat(VerticalWalls(maze, grid)))
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

    private static IEnumerable<SvgPathSegment> HorizontalWalls(Maze maze, Grid grid)
    {
        yield return new SvgMoveToSegment(false, TopLeft(new Cell(0, 0)));

        foreach (var cell in grid.Cells.Where(c => c.Y == 0))
        {
            yield return WallOrBlank(WallExists(maze, cell, Direction.North, grid), TopRight(cell));
        }

        foreach (var grouping in grid.Cells.GroupBy(c => c.Y))
        {
            yield return new SvgMoveToSegment(false, BottomLeft(new Cell(0, grouping.Key)));

            foreach (var cell in grouping)
            {
                yield return WallOrBlank(WallExists(maze, cell, Direction.South, grid), BottomRight(cell));
            }
        }
    }

    private static IEnumerable<SvgPathSegment> VerticalWalls(Maze maze, Grid grid)
    {
        yield return new SvgMoveToSegment(false, TopLeft(new Cell(0, 0)));

        foreach (var cell in grid.Cells.Where(c => c.X == 0))
        {
            yield return WallOrBlank(WallExists(maze, cell, Direction.West, grid), BottomLeft(cell));
        }

        foreach (var grouping in grid.Cells.GroupBy(c => c.X))
        {
            yield return new SvgMoveToSegment(false, TopRight(new Cell(grouping.Key, 0)));

            foreach (var cell in grouping)
            {
                yield return WallOrBlank(WallExists(maze, cell, Direction.East, grid), BottomRight(cell));
            }
        }
    }

    private static bool WallExists(Maze maze, Cell cell, Direction direction, Grid grid)
    {
        var adjacentCell = grid.AdjacentCellOrNull(cell, direction);

        if (adjacentCell == null)
        {
            // If there isn't an adjacent cell, then it must mean we're at the edge of the maze, and walls exist around
            // the entire boundary.
            return true;
        }

        return !maze.EdgeExistsBetween(cell, adjacentCell);
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
