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
            from maze in BinaryTree.MazeDistribution(Grid)
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

            var mazeWithAllWalls = Maze.WithAllWalls(Grid);

            File.WriteAllText(
                path: Path.Combine(folderPath, "maze-all-walls.svg"),
                MazeImage.CreateSvg(mazeWithAllWalls).GetXML());

            var binaryTreeDistribution = BinaryTree.MazeDistribution(Grid);

            var binaryTreeMaze = binaryTreeDistribution.Sample(this.rng);

            File.WriteAllText(
                path: Path.Combine(folderPath, "binary-tree.svg"),
                MazeImage.CreateSvg(binaryTreeMaze).GetXML());

            var repeatedDistribution = binaryTreeDistribution.Repeat(5);

            var mazes = repeatedDistribution.Sample(this.rng);

            using (var gif = AnimatedGif.Create(Path.Combine(folderPath, "mazes.gif"), delay: 1000))
            {
                foreach (var maze in mazes)
                {
                    var image = MazeImage.CreateSvg(maze).Draw();

                    gif.AddFrame(image, quality: GifQuality.Bit8);
                }
            }

            var steps = BinaryTree.MazesDistribution(Grid).Sample(this.rng);

            using (var gif = AnimatedGif.Create(Path.Combine(folderPath, "construction.gif"), delay: 100))
            {
                foreach (var maze in steps)
                {
                    var image = MazeImage.CreateSvg(maze).Draw();

                    gif.AddFrame(image, quality: GifQuality.Bit8);
                }
            }
        }
    }
}
