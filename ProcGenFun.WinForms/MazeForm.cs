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
            from maze in Sidewinder.MazeDistribution(Grid)
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

            var sidewinderDistribution = Sidewinder.MazeDistribution(Grid);

            var sidewinderMaze = sidewinderDistribution.Sample(this.rng);

            File.WriteAllText(
                path: Path.Combine(folderPath, "sidewinder.svg"),
                MazeImage.CreateSvg(sidewinderMaze).GetXML());

            var repeatedDistribution = sidewinderDistribution.Repeat(5);

            var mazes = repeatedDistribution.Sample(this.rng);

            using (var gif = AnimatedGif.Create(Path.Combine(folderPath, "mazes.gif"), delay: 1000))
            {
                foreach (var maze in mazes)
                {
                    var image = MazeImage.CreateSvg(maze).Draw();

                    gif.AddFrame(image, quality: GifQuality.Bit8);
                }
            }
        }
    }
}
