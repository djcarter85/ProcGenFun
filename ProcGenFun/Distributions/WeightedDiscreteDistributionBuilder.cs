namespace ProcGenFun.Distributions;

using System.Collections.Immutable;
using RandN;

public class WeightedDiscreteDistributionBuilder<T>
{
    private readonly ImmutableList<Weighting<T>> weightings;

    private WeightedDiscreteDistributionBuilder(ImmutableList<Weighting<T>> weightings)
    {
        this.weightings = weightings;
    }

    public static WeightedDiscreteDistributionBuilder<T> Empty() => new(ImmutableList<Weighting<T>>.Empty);
    
    public WeightedDiscreteDistributionBuilder<T> Add(T value, int weighting) =>
        new(this.weightings.Add(new Weighting<T>(value, weighting)));
    
    public IDistribution<T> Build() => WeightedDiscreteDistribution.New(weightings);
}