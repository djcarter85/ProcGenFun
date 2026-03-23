namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class VerticalDibandCreator
{
    public static IDistribution<Flag> Dist()
    {
        var chargeLocationDist = WeightedDiscreteDistributionBuilder<FlagChargeHorizontalLocation?>.Empty()
            .Add(null, 4)
            .Add(FlagChargeHorizontalLocation.Left, 1)
            .Add(FlagChargeHorizontalLocation.Right, 1)
            .Build();
        
        return from left in FlagColours.AllDist()
            from right in FlagColours.AllowedAdjacentToDist(left)
            from chargeLocation in chargeLocationDist
            from charges in VerticalDibandChargesDist(chargeLocation, left, right)
            select new Flag(new FlagPattern.VerticalDiband(left, right), charges);

        IDistribution<IReadOnlyList<FlagCharge>> VerticalDibandChargesDist(
            FlagChargeHorizontalLocation? chargeLocation, FlagColour left, FlagColour right) =>
            chargeLocation switch
            {
                FlagChargeHorizontalLocation.Left =>
                    from colour in FlagColours.AllowedAdjacentToDist(left)
                    select (IReadOnlyList<FlagCharge>)
                    [
                        new FlagCharge(new FlagChargeShape.Star(colour),
                            1.5f,
                            FlagChargeHorizontalLocation.Left,
                            FlagChargeVerticalLocation.Centre)
                    ],
                FlagChargeHorizontalLocation.Centre =>
                    throw new ArgumentOutOfRangeException(nameof(chargeLocation), chargeLocation, null),
                FlagChargeHorizontalLocation.Right => 
                    from colour in FlagColours.AllowedAdjacentToDist(right)
                    select (IReadOnlyList<FlagCharge>)
                    [
                        new FlagCharge(new FlagChargeShape.Star(colour),
                            1.5f,
                            FlagChargeHorizontalLocation.Right,
                            FlagChargeVerticalLocation.Centre)
                    ],
                null => Singleton.New<IReadOnlyList<FlagCharge>>([]),
                _ => throw new ArgumentOutOfRangeException(nameof(chargeLocation), chargeLocation, null)
            };
    }
}