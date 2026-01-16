namespace ProcGenFun.WinForms;

using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public partial class ClockForm : Form
{
    private readonly IRng rng;

    public ClockForm(IRng rng)
    {
        this.rng = rng;

        this.InitializeComponent();
    }

    private void EstimateProbabilitiesButton_Click(object sender, EventArgs e)
    {
        var lastNumberFoundDist = LastClockNumberReached();

        var probabilityDensityDist = lastNumberFoundDist.EstimateProbabilityDensities(sampleCount: 10_000);

        var probabilityDensity = probabilityDensityDist.Sample(this.rng);

        this.outputLabel.Text = string.Join(
            Environment.NewLine,
            probabilityDensity.OrderBy(kvp => kvp.Key).Select(kvp => $"{kvp.Key}: {kvp.Value}%"));
    }

    private static IDistribution<int> LastClockNumberReached()
    {
        var modulus = 12;

        var relativeStepDist =
            from x in Bernoulli.FromRatio(1, 2)
            select x ? +1 : -1;

        var randomWalkDist = RandomWalk.New(
            initial: 0,
            stepDist: curr => from rs in relativeStepDist select Modulo(curr + rs, modulus));

        return
            from rw in randomWalkDist
            select GetLastNumberReached(rw, modulus);
    }

    private static int GetLastNumberReached(IEnumerable<int> numbers, int modulus)
    {
        var unreached = Enumerable.Range(0, modulus).ToList();
        foreach (var number in numbers)
        {
            unreached.Remove(number);
            if (unreached.Count == 1)
            {
                return unreached[0];
            }
        }

        throw new InvalidOperationException();
    }

    private static int Modulo(int a, int b)
    {
        var n = a % b;
        return n < 0 ? n + b : n;
    }
}
