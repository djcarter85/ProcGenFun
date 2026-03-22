namespace ProcGenFun.WinForms;

using ProcGenFun.Perlin;
using RandN;
using RandN.Extensions;

public partial class PerlinForm : Form
{
    private const double InitialAmplitude = 1d;
    private const double MinX = 0d;
    private const double MaxX = 1000d;
    private const int Layers = 10;
    private const int MaxIndex = Layers - 1;

    private static readonly IDistribution<IReadOnlyList<IFunction1>> LayersDist =
        from perlin in Perlin.LayeredPerlin1Dist(min: MinX, max: MaxX, initialAmplitude: InitialAmplitude, layers: Layers)
        select perlin.ToReadOnly();

    private readonly IRng rng;

    private IReadOnlyList<IFunction1> layers;
    private int index = 0;

    public PerlinForm(IRng rng)
    {
        this.rng = rng;

        this.InitializeComponent();

        this.SetAxisLimits();
    }

    private void ReRollButton_Click(object sender, EventArgs e)
    {
        layers = LayersDist.Sample(this.rng);
        index = MaxIndex;
        DrawFunction(layers[index]);
    }

    private void DrawFunction(IFunction1 function1)
    {
        var numberOfPoints = 10_000;
        var period = (MaxX - MinX) / numberOfPoints;

        var ys = Enumerable.Range(0, numberOfPoints + 1).Select(x => function1.Evaluate(x * period)).ToArray();

        this.formsPlot.Reset();
        this.formsPlot.Plot.Add.Signal(ys, period);
        this.SetAxisLimits();
        this.formsPlot.Refresh();
    }

    private void SetAxisLimits()
    {
        this.formsPlot.Plot.Axes.Left.Min = -2 * InitialAmplitude;
        this.formsPlot.Plot.Axes.Left.Max = 2 * InitialAmplitude;

        this.formsPlot.Plot.Axes.Bottom.Min = MinX;
        this.formsPlot.Plot.Axes.Bottom.Max = MaxX;
    }

    private void BackButton_Click(object sender, EventArgs e)
    {
        index = Math.Max(index - 1, 0);
        DrawFunction(layers[index]);
    }

    private void ForwardButton_Click(object sender, EventArgs e)
    {
        index = Math.Min(index + 1, MaxIndex);
        DrawFunction(layers[index]);
    }
}
