namespace ProcGenFun.WinForms;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
    }

    private void VisualiseDistributionsButton_Click(object sender, EventArgs e)
    {
        new DistributionsForm().Show();
    }
}
