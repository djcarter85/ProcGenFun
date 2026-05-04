namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;

public static class FlagCreator
{
    public static IDistribution<Flag> FlagDist() =>
        WeightedDiscreteDistributionBuilder<IDistribution<Flag>>.Empty()
            .Add(SolidFlagCreator.Dist(), 15)
            .Add(CantonFlagCreator.Dist(), 15)
            .Add(VerticalBisectionCreator.Dist(), 25)
            .Add(HorizontalBisectionCreator.Dist(), 35)
            .Add(VerticalTribandCreator.Dist(), 50)
            .Add(HorizontalTribandCreator.Dist(), 45)
            .Add(DiagonalBisectionCreator.Dist(), 10)
            .Add(DiagonalBandCreator.Dist(), 10)
            .Add(CrossCreator.Dist(), 25)
            .Add(SaltireCreator.Dist(), 15)
            .Add(QuadrisectionCreator.Dist(), 10)
            .Add(HorizontalStripedCreator.Dist(), 10)
            .Add(PallCreator.Dist(), 15)
            .Build()
            .Flatten();
}