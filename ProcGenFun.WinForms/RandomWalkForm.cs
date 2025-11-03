namespace ProcGenFun.WinForms;

using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public partial class RandomWalkForm : Form
{
    private readonly IRng rng;

    public RandomWalkForm(IRng rng)
    {
        this.rng = rng;

        this.InitializeComponent();
    }

    private void RandomWalkForm_Load(object sender, EventArgs e)
    {
        IDistribution<int> StepDist(int current) =>
            from b in Bernoulli.FromRatio(numerator: 1, denominator: 2)
            let step = b ? +1 : -1
            select current + step;

        var randomWalkDist = RandomWalk.New<int>(initial: 0, stepDist: StepDist);

        // This returns an infinite random walk.
        var randomWalk = randomWalkDist.Sample(this.rng);

        // Take 100 steps as an example.
        var steps = randomWalk.Take(100);

        this.outputTextBox.Text = string.Join(", ", steps);
    }
}
