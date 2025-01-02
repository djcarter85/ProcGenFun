namespace ProcGenFun.WinForms;

using AnimatedGif;
using ProcGenFun.Distributions;
using ProcGenFun.Mazes;
using RandN;
using RandN.Extensions;
using Svg;
using System.Collections.Immutable;
using System.Windows.Forms.VisualStyles;

public partial class MazeForm : Form
{
    private static readonly Grid Grid = new(width: 16, height: 10);

    private readonly IRng rng;

    private IReadOnlyList<Image>? images;
    private int selectedImageIndex;

    public MazeForm(IRng rng)
    {
        this.rng = rng;

        InitializeComponent();

        this.mazeAlgorithmCombo.Items.Add(MazeAlgorithm.BinaryTree);
        this.mazeAlgorithmCombo.Items.Add(MazeAlgorithm.Sidewinder);
        this.mazeAlgorithmCombo.SelectedIndex = 0;
    }

    private void GenerateButton_Click(object sender, EventArgs e)
    {
        var imagesDist = (MazeAlgorithm)this.mazeAlgorithmCombo.SelectedItem switch
        {
            MazeAlgorithm.BinaryTree => from history in BinaryTree.HistoryDist(Grid) select GetImages(history),
            MazeAlgorithm.Sidewinder => from history in Sidewinder.HistoryDist(Grid) select GetImages(history),
            _ => throw new ArgumentOutOfRangeException()
        };

        this.images = imagesDist.Sample(this.rng);

        this.DisplayImage(index: 0);
    }

    private void DisplayImage(int index)
    {
        if (this.images != null)
        {
            this.selectedImageIndex = index;

            var image = this.images[index];

            this.pictureBox.Image = image;
            this.pictureBox.Size = image.Size;

            this.firstButton.Enabled = index != 0;
            this.previousButton.Enabled = index != 0;
            this.nextButton.Enabled = index != this.images.Count - 1;
            this.lastButton.Enabled = index != this.images.Count - 1;

            this.label1.Text = $"{index + 1} of {this.images.Count}";
        }
    }

    private static IReadOnlyList<Image> GetImages(BinaryTreeHistory history)
    {
        var images = new List<Image>();

        images.Add(MazeImage.CreateSvg(history.Initial).Draw());

        foreach (var step in history.Steps)
        {
            images.Add(MazeImage.CreateSvg(step.Maze, highlightedCells: [step.Cell]).Draw());
        }

        images.Add(MazeImage.CreateSvg(history.Final).Draw());

        return images;
    }

    private static IReadOnlyList<Image> GetImages(SidewinderHistory history)
    {
        var images = new List<Image>();

        images.Add(MazeImage.CreateSvg(history.Initial).Draw());

        foreach (var step in history.Steps)
        {
            images.Add(MazeImage.CreateSvg(step.Maze, highlightedCells: step.Run).Draw());
        }

        images.Add(MazeImage.CreateSvg(history.Current).Draw());

        return images;
    }

    private void SaveImagesButton_Click(object sender, EventArgs e)
    {
        var folderBrowserDialog = new FolderBrowserDialog();

        var dialogResult = folderBrowserDialog.ShowDialog();

        if (dialogResult == DialogResult.OK)
        {
            var folderPath = folderBrowserDialog.SelectedPath;

            Directory.Delete(folderPath, true);
            Directory.CreateDirectory(folderPath);

            switch ((MazeAlgorithm)this.mazeAlgorithmCombo.SelectedItem)
            {
                case MazeAlgorithm.BinaryTree:
                    {
                        var historyDist = BinaryTree.HistoryDist(Grid);

                        var history = historyDist.Sample(this.rng);

                        SaveMazeWithAllWallsImage(folderPath);

                        SaveMazeImage(folderPath, history.Final);

                        SaveMazeAnimation(
                            folderPath,
                            history.Initial,
                            history.Steps.Select(s => new MazeHighlight(s.Maze, [s.Cell])),
                            history.Final);
                    }

                    break;

                case MazeAlgorithm.Sidewinder:
                    {
                        var historyDist = Sidewinder.HistoryDist(Grid);

                        var history = historyDist.Sample(this.rng);

                        SaveMazeWithAllWallsImage(folderPath);

                        SaveMazeImage(folderPath, history.Current);

                        SaveMazeAnimation(
                            folderPath,
                            history.Initial,
                            history.Steps.Select(s => new MazeHighlight(s.Maze, s.Run)),
                            history.Current);

                        SaveMazeImages(
                            folderPath,
                            history.Initial,
                            history.Steps.Select(s => new MazeHighlight(s.Maze, s.Run)),
                            history.Current);
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
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
            path: Path.Combine(folderPath, "binary-tree.svg"),
            MazeImage.CreateSvg(maze).GetXML());
    }

    private static void SaveMazeAnimation(
        string folderPath,
        Maze initial,
        IEnumerable<MazeHighlight> steps,
        Maze final)
    {
        using var gif = AnimatedGif.Create(Path.Combine(folderPath, "maze-animation.gif"), delay: 75);

        void AddFrame(Maze maze, IReadOnlyList<Cell>? highlightedCells = null, int delay = -1) =>
            gif.AddFrame(
                MazeImage.CreateSvg(maze, highlightedCells: highlightedCells ?? []).Draw(),
                quality: GifQuality.Bit8,
                delay: delay);

        AddFrame(initial, delay: 1000);

        foreach (var step in steps)
        {
            AddFrame(step.Maze, highlightedCells: step.HighlightedCells);
        }

        AddFrame(final, delay: 1000);
    }

    private static void SaveMazeImages(
        string folderPath,
        Maze initial,
        IEnumerable<MazeHighlight> steps,
        Maze final)
    {
        void SaveImage(Maze maze, string suffix, IReadOnlyList<Cell>? highlightedCells = null) =>
            File.WriteAllText(
                Path.Combine(folderPath, $"maze_{suffix}.svg"),
                MazeImage.CreateSvg(maze, highlightedCells: highlightedCells ?? []).GetXML());

        SaveImage(initial, suffix: "A");

        int i = 0;
        foreach (var step in steps)
        {
            SaveImage(step.Maze, suffix: $"B_{i:0000}", highlightedCells: step.HighlightedCells);
            i++;
        }

        SaveImage(final, suffix: "C");
    }

    private void firstButton_Click(object sender, EventArgs e)
    {
        this.DisplayImage(0);
    }

    private void previousButton_Click(object sender, EventArgs e)
    {
        this.DisplayImage(this.selectedImageIndex - 1);
    }

    private void nextButton_Click(object sender, EventArgs e)
    {
        this.DisplayImage(this.selectedImageIndex + 1);
    }

    private void lastButton_Click(object sender, EventArgs e)
    {
        this.DisplayImage(this.images.Count - 1);
    }
}
