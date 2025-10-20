namespace ProcGenFun.WinForms;

using AnimatedGif;
using ProcGenFun.Distributions;
using ProcGenFun.Mazes;
using RandN;
using RandN.Extensions;
using Svg;

public partial class MazeForm : Form
{
    private static readonly Grid Grid = new(width: 16, height: 10);

    private readonly IRng rng;

    public MazeForm(IRng rng)
    {
        this.rng = rng;

        InitializeComponent();

        this.algorithmCombo.SelectedIndex = 0;
    }

    private void GenerateButton_Click(object sender, EventArgs e)
    {
        var mazeDist = algorithmCombo.SelectedIndex switch
        {
            0 => BinaryTree.MazeDist(Grid),
            1 => Sidewinder.MazeDist(Grid),
            2 => RecursiveBacktracker.MazeDist(Grid),
            _ => throw new NotImplementedException(),
        };

        var imageDist =
            from maze in mazeDist
            let svg = MazeImage.CreateSvg(maze)
            select svg.Draw();

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

                SaveMazeWithAllWallsImage(folderPath);

                SaveMazeImage(folderPath, history.Final);

                SaveMazeAnimationAndFrames(
                    folderPath,
                    history.Initial,
                    HighlightedMazesCreator.FromBinaryTreeHistory(history),
                    history.Final);
            }
            else
            {
                var historyDist = Sidewinder.HistoryDist(Grid);

                var history = historyDist.Sample(this.rng);

                SaveMazeWithAllWallsImage(folderPath);

                SaveMazeImage(folderPath, history.Current);

                SaveMazeAnimationAndFrames(
                    folderPath,
                    history.Initial,
                    HighlightedMazesCreator.FromSidewinderHistory(history),
                    history.Current);
            }
        }
    }

    private static void SaveMazeWithAllWallsImage(string folderPath)
    {
        var mazeWithAllWalls = Maze.WithAllWalls(Grid);

        File.WriteAllText(
            path: Path.Combine(folderPath, "maze-all-walls.svg"),
            MazeImage.CreateSvg(mazeWithAllWalls).GetXML());
    }

    private static void SaveMazeImage(string folderPath, Maze maze)
    {
        File.WriteAllText(
            path: Path.Combine(folderPath, "maze.svg"),
            MazeImage.CreateSvg(maze).GetXML());
    }

    private static void SaveMazeAnimationAndFrames(
        string folderPath,
        Maze initial,
        IEnumerable<HighlightedMaze> steps,
        Maze final)
    {
        var framesPath = Path.Combine(folderPath, "frames");
        Directory.CreateDirectory(framesPath);

        using var gif = AnimatedGif.Create(Path.Combine(folderPath, "maze-animation.gif"), delay: 75);

        var index = 0;

        void AddFrame(Maze maze, IReadOnlyList<Cell>? highlightedCells = null, int delay = -1)
        {
            var svgDocument = MazeImage.CreateSvg(maze, highlightedCells: highlightedCells ?? []);

            File.WriteAllText(
                Path.Combine(framesPath, $"frame_{index++:0000}.svg"),
                svgDocument.GetXML());
            gif.AddFrame(
                svgDocument.Draw(),
                quality: GifQuality.Bit8,
                delay: delay);
        }

        AddFrame(initial, delay: 1000);

        foreach (var step in steps)
        {
            AddFrame(step.Maze, highlightedCells: step.HighlightedCells);
        }

        AddFrame(final, delay: 1000);
    }
}
