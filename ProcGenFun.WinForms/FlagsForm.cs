namespace ProcGenFun.WinForms;

using ProcGenFun.Distributions;
using ProcGenFun.Flags;
using ProcGenFun.Flags.Generation;
using ProcGenFun.Flags.Images;
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

    private void CreateFlagsButton_Click(object sender, EventArgs e)
    {
        var imageDist =
            from flag in FlagCreator.FlagDist()
            let svg = FlagImage.CreateSvg(flag)
            select svg.Draw(rasterWidth: 180, rasterHeight: 120);

        var imagesDist = imageDist.Repeat(9);

        var images = imagesDist.Sample(this.rng);

        foreach (var pictureBox in this.Controls.OfType<PictureBox>().ToArray())
        {
            this.Controls.Remove(pictureBox);
            pictureBox.Dispose();
        }

        var index = 0;
        foreach (var image in images)
        {
            var row = index / 3;
            var col = index % 3;

            var gap = 50;

            var pictureBox = new PictureBox
            {
                Location = new Point(
                    12 + row * (image.Width + gap),
                    87 + col * (image.Height + gap)),
                Image = image,
                Width = image.Width,
                Height = image.Height
            };

            this.Controls.Add(pictureBox);

            index++;
        }
    }
}
