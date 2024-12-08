namespace ProcGenFun.WinForms;

using Svg;

public partial class MazeForm : Form
{
    private const string OutputPath = @"C:\a\mazes\maze.svg";

    public MazeForm()
    {
        InitializeComponent();
    }

    private void GenerateAndSaveButton_Click(object sender, EventArgs e)
    {
        var grid = new Grid(width: 16, height: 10);
        var maze = Maze.WithAllWalls(grid);
        var svg = MazeImage.CreateSvg(maze);

        File.WriteAllText(path: OutputPath, svg.GetXML());
    }
}
