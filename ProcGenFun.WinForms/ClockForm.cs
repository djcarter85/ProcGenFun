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
        var probabilityDensityDist =
            LastNumberReachedDist()
                .EstimateProbabilityDensities(sampleCount: 100_000);

        var probabilityDensity = probabilityDensityDist.Sample(this.rng);

        this.outputLabel.Text = string.Join(
            Environment.NewLine,
            probabilityDensity.OrderBy(kvp => kvp.Key).Select(kvp => $"{kvp.Key}: {kvp.Value}%"));
    }

    private static IDistribution<int> LastNumberReachedDist()
    {
        var modulus = 12;

        IDistribution<int> StepDist(int current) =>
            from x in Bernoulli.FromRatio(1, 2)
            let relativeStep = x ? +1 : -1
            select Modulo(current + relativeStep, modulus);

        var randomWalkDist = RandomWalk.New(initial: 0, StepDist);

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
