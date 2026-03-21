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
        var perlinDist = Perlin.Perlin1Dist(min: 0d, max: 1000d, frequency: 4);
        var period = 0.1d;

        var ysDist =
            from perlin in perlinDist
            select Enumerable.Range(0, 10_001).Select(x => perlin.Evaluate(x* period)).ToArray();

        var ys = ysDist.Sample(this.rng);

        this.formsPlot.Reset();
        this.formsPlot.Plot.Add.Signal(ys, period);
        this.formsPlot.Refresh();
    }
}
