namespace ProcGenFun.WinForms;

using ProcGenFun.Maps;
using RandN;
using RandN.Extensions;

public partial class PerlinForm : Form
{
    private const double InitialAmplitude = 1d;
    private const double MinX = 0d;
    private const double MaxX = 1000d;
    
    private readonly IRng rng;

    public PerlinForm(IRng rng)
    {
        this.rng = rng;

        this.InitializeComponent();
        
        this.SetAxisLimits();
    }

    private void ReRollButton_Click(object sender, EventArgs e)
    {
        var perlinDist = Perlin.Perlin1Dist(min: MinX, max: MaxX, frequency: 1, amplitude: InitialAmplitude);

        var numberOfPoints = 10_000;
        var period = (MaxX - MinX) / numberOfPoints;

        var ysDist =
            from perlin in perlinDist
            select Enumerable.Range(0, numberOfPoints + 1).Select(x => perlin.Evaluate(x * period)).ToArray();

        var ys = ysDist.Sample(this.rng);

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
}
