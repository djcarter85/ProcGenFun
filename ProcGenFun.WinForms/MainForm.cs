namespace ProcGenFun.WinForms;

using RandN;

public partial class MainForm : Form
{
    private readonly IRng rng;

    public MainForm(IRng rng)
    {
        this.rng = rng;

        InitializeComponent();
    }

    private void VisualiseDistributionsButton_Click(object sender, EventArgs e)
    {
        new DistributionsForm(this.rng).Show();
    }

    private void MazesButton_Click(object sender, EventArgs e)
    {
        new MazeForm(this.rng).Show();
    }

    private void BirthdaysButton_Click(object sender, EventArgs e)
    {
        new BirthdaysForm(this.rng).Show();
    }
}
