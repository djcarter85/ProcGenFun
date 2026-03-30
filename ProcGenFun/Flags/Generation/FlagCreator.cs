namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Extensions;

public static class FlagCreator
{
    public static IDistribution<Flag> FlagDist() =>
        WeightedDiscreteDistributionBuilder<IDistribution<Flag>>.Empty()
            .Add(SolidFlagCreator.Dist(), 15)
            .Add(CantonFlagCreator.Dist(), 15)
            .Add(VerticalDibandCreator.Dist(), 25)
            .Add(HorizontalDibandCreator.Dist(), 35)
            .Add(VerticalTribandCreator.Dist(), 50)
            .Add(HorizontalTribandCreator.Dist(), 45)
            .Add(DiagonalBicolourCreator.Dist(), 10)
            .Add(CrossCreator.Dist(), 25)
            .Add(SaltireCreator.Dist(), 15)
            .Add(QuarteredCreator.Dist(), 10)
            .Add(HorizontalStripedCreator.Dist(), 10)
            .Add(PallCreator.Dist(), 15)
            .Build()
            .Flatten();
}