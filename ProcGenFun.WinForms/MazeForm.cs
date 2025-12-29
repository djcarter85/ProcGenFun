namespace ProcGenFun.WinForms;

using AnimatedGif;
using ProcGenFun.Distributions;
using ProcGenFun.Mazes;
using RandN;
using RandN.Distributions;
using RandN.Extensions;
using Svg;

public partial class MazeForm : Form
{
    private static readonly RectGrid Grid = new(width: 16, height: 10);
    private static readonly HexGrid HexGrid = new(maxDistanceFromOrigin: 6);

    private readonly IRng rng;

    public MazeForm(IRng rng)
    {
        this.rng = rng;

        InitializeComponent();

        this.algorithmCombo.SelectedIndex = 0;
    }

    private void GenerateButton_Click(object sender, EventArgs e)
    {
        var imageDist = algorithmCombo.SelectedIndex switch
        {
            0 =>
                from maze in BinaryTree.MazeDist(Grid)
                let svg = RectMazeImage.CreateSvg(maze, CellColours.Base<RectCell>(), Grid)
                select svg.Draw(),
            1 =>
                from maze in Sidewinder.MazeDist(Grid)
                let svg = RectMazeImage.CreateSvg(maze, CellColours.Base<RectCell>(), Grid)
                select svg.Draw(),
            2 =>
                from maze in RecursiveBacktracker.MazeDist(Grid.Cells, Grid.GetNeighbours)
                let svg = RectMazeImage.CreateSvg(maze, CellColours.Base<RectCell>(), Grid)
                select svg.Draw(),
            3 =>
                Singleton.New(HexMazeImage.CreateSvg(HexGrid).Draw()),
            _ => throw new NotImplementedException(),
        };

        var image = imageDist.Sample(this.rng);

        this.pictureBox.Image = image;
        this.pictureBox.Size = image.Size;
    }

    private void SaveImagesButton_Click(object sender, EventArgs e)
    {
        var folderBrowserDialog = new FolderBrowserDialog();

        var dialogResult = folderBrowserDialog.ShowDialog();

        if (dialogResult == DialogResult.OK)
        {
            var folderPath = folderBrowserDialog.SelectedPath;

            if (algorithmCombo.SelectedIndex == 0)
            {
                var historyDist = BinaryTree.HistoryDist(Grid);

                var history = historyDist.Sample(this.rng);

                SaveRectMazeWithAllWallsImage(folderPath);

                SaveRectMazeImage(folderPath, history.Final);

                SaveMazeAnimationAndFrames(
                    folderPath,
                    history.Initial,
                    ColouredMazeCreator.FromBinaryTreeHistory(history),
                    history.Final,
                    CellColours.Base<RectCell>(),
                    (m, gcc) => RectMazeImage.CreateSvg(m, gcc, Grid));
            }
            else if (algorithmCombo.SelectedIndex == 1)
            {
                var historyDist = Sidewinder.HistoryDist(Grid);

                var history = historyDist.Sample(this.rng);

                SaveRectMazeWithAllWallsImage(folderPath);

                SaveRectMazeImage(folderPath, history.Current);

                SaveMazeAnimationAndFrames(
                    folderPath,
                    history.Initial,
                    ColouredMazeCreator.FromSidewinderHistory(history),
                    history.Current,
                    CellColours.Base<RectCell>(),
                    (m, gcc) => RectMazeImage.CreateSvg(m, gcc, Grid));
            }
            else if (algorithmCombo.SelectedIndex == 2)
            {
                var historyDist = RecursiveBacktracker.HistoryDist(Grid.Cells, Grid.GetNeighbours);

                var history = historyDist.Sample(this.rng);

                SaveRectMazeWithAllWallsImage(folderPath);

                SaveRectMazeImage(folderPath, history.Last().Maze);

                SaveMazeAnimationAndFrames(
                    folderPath,
                    history.First().Maze,
                    ColouredMazeCreator.FromRecursiveBacktrackerHistory(history),
                    history.Last().Maze,
                    CellColours.RBUnvisited<RectCell>(),
                    (m, gcc) => RectMazeImage.CreateSvg(m, gcc, Grid));
            }
            else
            {
                SaveHexMazeWithAllWallsImage(folderPath);
            }
        }
    }

    private static void SaveHexMazeWithAllWallsImage(string folderPath)
    {
        File.WriteAllText(
            path: Path.Combine(folderPath, "maze-all-walls.svg"),
            HexMazeImage.CreateSvg(HexGrid).GetXML());
    }

    private static void SaveRectMazeWithAllWallsImage(string folderPath)
    {
        var mazeWithAllWalls = Maze.WithNoEdges(Grid.Cells);

        File.WriteAllText(
            path: Path.Combine(folderPath, "maze-all-walls.svg"),
            RectMazeImage.CreateSvg(mazeWithAllWalls, CellColours.Base<RectCell>(), Grid).GetXML());
    }

    private static void SaveRectMazeImage(string folderPath, Maze<RectCell> maze)
    {
        File.WriteAllText(
            path: Path.Combine(folderPath, "maze.svg"),
            RectMazeImage.CreateSvg(maze, CellColours.Base<RectCell>(), Grid).GetXML());
    }

    private static void SaveMazeAnimationAndFrames<TCell>(
        string folderPath,
        Maze<TCell> initial,
        IEnumerable<ColouredMaze<TCell>> steps,
        Maze<TCell> final,
        Func<TCell, Color> initialCellColours,
        Func<Maze<TCell>, Func<TCell, Color>, SvgDocument> createSvg) where TCell : notnull
    {
        var framesPath = Path.Combine(folderPath, "frames");
        Directory.CreateDirectory(framesPath);

        using var gif = AnimatedGif.Create(Path.Combine(folderPath, "maze-animation.gif"), delay: 75);

        var index = 0;

        void AddFrame(Maze<TCell> maze, Func<TCell, Color> getCellColor, int delay = -1)
        {
            var svgDocument = createSvg(maze, getCellColor);

            File.WriteAllText(
                Path.Combine(framesPath, $"frame_{index++:0000}.svg"),
                svgDocument.GetXML());
            gif.AddFrame(
                svgDocument.Draw(),
                quality: GifQuality.Bit8,
                delay: delay);
        }

        AddFrame(initial, initialCellColours, delay: 1000);

        foreach (var step in steps)
        {
            AddFrame(step.Maze, step.GetCellColour);
        }

        AddFrame(final, CellColours.Base<TCell>(), delay: 1000);
    }
}
