namespace ProcGenFun.WinForms;

using ProcGenFun.Maps;
using RandN;
using RandN.Extensions;

public partial class MapsForm : Form
{
    private readonly IRng rng;

    public MapsForm(IRng rng)
    {
        this.rng = rng;

        this.InitializeComponent();
    }

    private void CreateMapButton_Click(object sender, EventArgs e)
    {
        var imageDist =
            from map in MapCreator.MapDist()
            let svg = MapImage.Create(map)
            select svg.Draw();

        var image = imageDist.Sample(this.rng);

        this.pictureBox.Width = image.Width;
        this.pictureBox.Height = image.Height;
        this.pictureBox.Image = image;
    }
}
