namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Extensions;

public static class HorizontalTribandCreator
{
    public static IDistribution<Flag> Dist()
    {
        var chargeTypeDist = WeightedDiscreteDistributionBuilder<FlagChargeShape.Type?>.Empty()
            .Add(null, 3)
            .Add(FlagChargeShape.Type.StarBand, 1)
            .Build();

        return from top in FlagColours.AllDist()
            from middle in FlagColours.AllowedAdjacentToDist(top)
            from bottom in FlagColours.AllowedAdjacentToDist(middle)
            from chargeType in chargeTypeDist
            from charge in FlagChargeCreator.ChargesDist(chargeType, backgroundColour: middle, size: 1.5f)
            select new Flag(new FlagPattern.HorizontalTriband(top, middle, bottom), charge);
    }
}