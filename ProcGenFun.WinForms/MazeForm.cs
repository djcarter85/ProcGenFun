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
    }

    private void GenerateButton_Click(object sender, EventArgs e)
    {
        var imageDist =
            from maze in BinaryTree.MazeDist(Grid)
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

            var historyDist = BinaryTree.HistoryDist(Grid);

            var history = historyDist.Sample(this.rng);

            SaveMazeWithAllWallsImage(folderPath);

            SaveMazeImage(folderPath, history.Final);

            SaveMazeAnimation(folderPath, history);
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
            path: Path.Combine(folderPath, "binary-tree.svg"),
            MazeImage.CreateSvg(maze).GetXML());
    }

    private static void SaveMazeAnimation(string folderPath, BinaryTreeHistory history)
    {
        using var gif = AnimatedGif.Create(Path.Combine(folderPath, "maze-animation.gif"), delay: 75);

        void AddFrame(Maze maze, Cell? highlightedCell = null, int delay = -1) =>
            gif.AddFrame(
                MazeImage.CreateSvg(maze, highlightedCell: highlightedCell).Draw(),
                quality: GifQuality.Bit8,
                delay: delay);

        AddFrame(history.Initial, delay: 1000);

        foreach (var snapshot in history.Steps)
        {
            AddFrame(snapshot.Maze, highlightedCell: snapshot.Cell);
        }

        AddFrame(history.Final, delay: 1000);
    }
}
