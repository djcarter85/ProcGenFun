namespace ProcGenFun.WinForms;

using RandN;
using RandN.Distributions;
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
        var dist = Uniform.New(0d, 1d);

        var values = TakeSamples(dist, count: 100_000, rng);

        DisplayHistogram(values);
    }

    private static IEnumerable<T> TakeSamples<T, TRng>(IDistribution<T> dist, int count, TRng rng)
        where TRng : notnull, IRng
    {
        for (int i = 0; i < count; i++)
        {
            yield return dist.Sample(rng);
        }
    }

    private void DisplayHistogram(IEnumerable<double> values)
    {
        var histogram = Histogram.New(values, min: 0, max: 1);

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
