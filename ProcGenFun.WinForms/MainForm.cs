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

        this.distributionTypeCombo.Items.Add(DistributionType.Uniform);
        this.distributionTypeCombo.Items.Add(DistributionType.Normal);

        this.distributionTypeCombo.SelectedIndex = 0;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        GenerateAndDisplayHistogram();
    }

    private void RegenerateButton_Click(object sender, EventArgs e)
    {
        GenerateAndDisplayHistogram();
    }

    private void GenerateAndDisplayHistogram()
    {
        var rng = StandardRng.Create();
        var dist = GetSelectedHistogramDistribution();

        var histogram = dist.Sample(rng);

        DisplayHistogram(histogram);
    }

    private IDistribution<Histogram> GetSelectedHistogramDistribution() =>
        (DistributionType)this.distributionTypeCombo.SelectedItem switch
        {
            DistributionType.Uniform =>
                Uniform.New(0d, 1d)
                    .Repeat(count: 100_000)
                    .Select(v => Histogram.New(v, min: 0, max: 1)),
            DistributionType.Normal =>
                Normal.New()
                    .Repeat(count: 100_000)
                    .Select(v => Histogram.New(v, min: -4, max: 4)),
            _ => throw new ArgumentOutOfRangeException(),
        };

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
