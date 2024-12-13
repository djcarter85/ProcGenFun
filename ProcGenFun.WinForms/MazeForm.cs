namespace ProcGenFun.WinForms;

using RandN;
using Svg;

public partial class MazeForm : Form
{
    private const string OutputPath = @"C:\a\mazes\maze.svg";

    private readonly IRng rng;

    public MazeForm(IRng rng)
    {
        this.rng = rng;

        InitializeComponent();
    }

    private void GenerateAndSaveButton_Click(object sender, EventArgs e)
    {
        var grid = new Grid(width: 16, height: 10);
        var mazeDist = BinaryTree.MazeDistribution(grid);

        var maze = mazeDist.Sample(this.rng);

        var svg = MazeImage.CreateSvg(maze);

        File.WriteAllText(path: OutputPath, svg.GetXML());
    }
}
