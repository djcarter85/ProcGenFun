namespace ProcGenFun.WinForms;

using RandN;

public partial class ClockForm : Form
{
    private readonly IRng rng;

    public ClockForm(IRng rng)
    {
        this.rng = rng;

        this.InitializeComponent();
    }

    private void EstimateProbabilitiesButton_Click(object sender, EventArgs e)
    {

    }
}
