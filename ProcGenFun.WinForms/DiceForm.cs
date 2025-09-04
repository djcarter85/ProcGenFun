namespace ProcGenFun.WinForms;

using ProcGenFun.Distributions;
using RandN;

public partial class DiceForm : Form
{
    private readonly IRng rng;

    public DiceForm(IRng rng)
    {
        this.rng = rng;

        this.InitializeComponent();
    }

    private void RollButton_Click(object sender, EventArgs e)
    {
        var rollDist = Dice.D6Dist().Repeat(5);

        var roll = rollDist.Sample(this.rng);

        this.rollLabel.Text = string.Join(", ", roll);
    }
}
