namespace ProcGenFun.WinForms;

using ProcGenFun.Flags;
using RandN;
using RandN.Extensions;

public partial class FlagsForm : Form
{
    private readonly IRng rng;

    public FlagsForm(IRng rng)
    {
        this.rng = rng;

        this.InitializeComponent();
    }

    private void CreateFlagButton_Click(object sender, EventArgs e)
    {
        var imageDist =
            from flag in FlagCreator.FlagDist()
            let svg = FlagImage.CreateSvg(flag)
            select svg.Draw();

        var image = imageDist.Sample(this.rng);

        this.pictureBox.Image = image;
        this.pictureBox.Size = image.Size;
    }
}
