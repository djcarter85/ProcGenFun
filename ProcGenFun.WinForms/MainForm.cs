namespace ProcGenFun.WinForms;

using RandN;
using RandN.Distributions;
using RandN.Extensions;
using ScottPlot;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        DisplayHistogram();
    }

    private void DisplayHistogram()
    {
        var rng = StandardRng.Create();
        var dist =
            Uniform.New(0d, 1d)
                .Repeat(count: 100_000)
                .Select(v => Histogram.New(v, min: 0, max: 1));

        var histogram = dist.Sample(rng);

        DisplayHistogram(histogram);
    }

    private void DisplayHistogram(Histogram histogram)
    {
        formsPlot.Reset();
        var barPlot = formsPlot.Plot.Add.Bars(
            histogram.Buckets.Select(b => b.Centre),
            histogram.Buckets.Select(b => b.Count));

        foreach (var bar in barPlot.Bars)
        {
            bar.Size = histogram.BucketWidth;
            bar.FillColor = Color.FromColor(Theme.Blue500);
        }

        formsPlot.Refresh();
    }
}
