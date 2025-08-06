namespace ProcGenFun.WinForms;

using ProcGenFun.Distributions;
using RandN;

public partial class BirthdaysForm : Form
{
    private readonly IRng rng;

    public BirthdaysForm(IRng rng)
    {
        this.rng = rng;

        this.InitializeComponent();
    }

    private void BirthdaysButton_Click(object sender, EventArgs e)
    {
        var probabilityDist = Birthdays.BirthdaySetDist(size: (int)this.groupSizeUpDown.Value)
            .EstimateProbability(
                x => Birthdays.PeopleShareBirthday(
                    x,
                    sharedBirthdayCount: (int)this.sharedBirthdaysUpDown.Value,
                    sharedBirthdayMustOccurDuringCamp: this.occurOnCampCheckBox.Checked),
                sampleCount: 1_000_000);

        var probability = probabilityDist.Sample(this.rng);

        this.probabilityLabel.Text = probability.ToString("P2");
    }
}