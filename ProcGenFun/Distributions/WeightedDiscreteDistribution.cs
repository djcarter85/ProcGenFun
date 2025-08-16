namespace ProcGenFun.Distributions;

using System;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class WeightedDiscreteDistribution
{
    public static IDistribution<T> New<T>(IEnumerable<Weighting<T>> weightings)
    {
        var unnormalisedWeightings = weightings.ToList();

        if (unnormalisedWeightings.Any(w => w.Weight < 0))
        {
            throw new ArgumentException("Negative weights are not allowed");
        }

        if (unnormalisedWeightings.All(w => w.Weight == 0))
        {
            throw new ArgumentException("At least one weight must be positive");
        }

        if (unnormalisedWeightings.Count == 1)
        {
            return Singleton.New(unnormalisedWeightings[0].Value);
        }

        if (unnormalisedWeightings.Count == 2)
        {
            return CreateBernoulliByWeights(
                unnormalisedWeightings[0], unnormalisedWeightings[1]);
        }

        var normalisedWeightings = NormaliseWeightings(
            unnormalisedWeightings, out var averageWeight);

        var underfullWeightings = new Stack<Weighting<T>>();
        var overfullWeightings = new Stack<Weighting<T>>();
        var distributions = new List<IDistribution<T>>();

        SetUpCategories();
        TransferNonExactWeights();

        return
            from dist in UniformDistribution.Create(distributions)
            from value in dist
            select value;

        void SetUpCategories()
        {
            foreach (var weighting in normalisedWeightings)
            {
                CategoriseWeighting(weighting);
            }
        }

        void CategoriseWeighting(Weighting<T> weighting)
        {
            if (weighting.Weight < averageWeight)
            {
                underfullWeightings.Push(weighting);
            }
            else if (weighting.Weight > averageWeight)
            {
                overfullWeightings.Push(weighting);
            }
            else
            {
                distributions.Add(Singleton.New(weighting.Value));
            }
        }

        void TransferNonExactWeights()
        {
            while (underfullWeightings.Any())
            {
                // "Under" and "over" refer to the average weight. So if there
                // is an underfull weighting there must be an overfull
                // weighting too.
                var underfullWeighting = underfullWeightings.Pop();
                var overfullWeighting = overfullWeightings.Pop();

                var transferredWeight = averageWeight - underfullWeighting.Weight;
                distributions.Add(
                    CreateBernoulliByWeights(
                        underfullWeighting,
                        overfullWeighting with { Weight = transferredWeight }));

                var adjustedWeight = overfullWeighting.Weight - transferredWeight;
                CategoriseWeighting(
                    overfullWeighting with { Weight = adjustedWeight });
            }
        }
    }

    private static IDistribution<T> CreateBernoulliByWeights<T>(
        Weighting<T> weighting1, Weighting<T> weighting2)
    {
        var numerator = (uint)weighting1.Weight;
        var denominator = (uint)(weighting1.Weight + weighting2.Weight);
        return Bernoulli.FromRatio(numerator, denominator)
            .Select(b => b ? weighting1.Value : weighting2.Value);
    }

    private static IReadOnlyList<Weighting<T>> NormaliseWeightings<T>(
        IReadOnlyList<Weighting<T>> unnormalisedWeightings, 
        out int averageWeight)
    {
        // We need the average weight to be an integer. One way to guarantee
        // this is to scale each weight by the number of weights in the list.
        // It would be possible (but more complicated) to work out a smaller
        // number to scale by in some situations.
        var scaleFactor = unnormalisedWeightings.Count;

        var normalisedWeightings = unnormalisedWeightings
            .Select(w => w with { Weight = w.Weight * scaleFactor })
            .ToList();

        averageWeight = unnormalisedWeightings.Sum(w => w.Weight);
        return normalisedWeightings;
    }
}
