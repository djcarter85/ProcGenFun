namespace ProcGenFun.Flags;

using ProcGenFun.Distributions;
using RandN;

public static class FlagColours
{
    private static readonly IEnumerable<FlagColour> all =
    [
        FlagColour.White,
        FlagColour.Red,
        FlagColour.DarkBlue,
        FlagColour.RoyalBlue,
        FlagColour.DarkGreen,
        FlagColour.Gold,
        FlagColour.Black,
        FlagColour.LightGreen,
        FlagColour.LightBlue,
        FlagColour.Maroon,
        FlagColour.Orange,
        FlagColour.Brown,
        FlagColour.Purple,
    ];

    private static readonly IEnumerable<ColourPairing> disallowedColourPairings =
    [
        new (FlagColour.Red, FlagColour.LightGreen),
        new (FlagColour.Red, FlagColour.DarkGreen),
        new (FlagColour.Red, FlagColour.Maroon),
        new (FlagColour.Red, FlagColour.Brown),
        new (FlagColour.RoyalBlue, FlagColour.Brown),
        new (FlagColour.RoyalBlue, FlagColour.Maroon),
        new (FlagColour.RoyalBlue, FlagColour.LightGreen),
        new (FlagColour.DarkBlue, FlagColour.DarkGreen),
    ];

    public static IDistribution<FlagColour> AllDist() => ColourDist(all);

    public static IDistribution<FlagColour> AllExceptDist(FlagColour exceptColour) =>
        ColourDist(all.Except([exceptColour]));

    public static IDistribution<FlagColour> AllowedAdjacentToDist(FlagColour adjacentColour) =>
        AllowedAdjacentToDist([adjacentColour]);

    public static IDistribution<FlagColour> AllowedAdjacentToDist(IEnumerable<FlagColour> adjacentColours) =>
        ColourDist(AllowedAdjacentTo(adjacentColours));

    private static IDistribution<FlagColour> ColourDist(IEnumerable<FlagColour> colours) =>
        WeightedDiscreteDistribution.New(colours.Select(c => new Weighting<FlagColour>(c, GetWeighting(c))));

    private static IEnumerable<FlagColour> AllowedAdjacentTo(IEnumerable<FlagColour> adjacentColours) =>
        all.Except(adjacentColours).Except(adjacentColours.SelectMany(DisallowedAdjacentTo));

    private static IEnumerable<FlagColour> DisallowedAdjacentTo(FlagColour adjacentColour) =>
        disallowedColourPairings.Where(p => p.Colour1 == adjacentColour).Select(p => p.Colour2)
            .Concat(disallowedColourPairings.Where(p => p.Colour2 == adjacentColour).Select(p => p.Colour1))
            .Distinct();

    private static int GetWeighting(FlagColour colour) =>
        colour switch
        {
            FlagColour.White => 100,
            FlagColour.Red => 95,
            FlagColour.DarkBlue => 90,
            FlagColour.RoyalBlue => 75,
            FlagColour.DarkGreen => 70,
            FlagColour.Gold => 70,
            FlagColour.Black => 65,
            FlagColour.LightGreen => 55,
            FlagColour.LightBlue => 40,
            FlagColour.Orange => 25,
            FlagColour.Maroon => 10,
            FlagColour.Brown => 10,
            FlagColour.Purple => 2,
            _ => throw new ArgumentOutOfRangeException(nameof(colour), colour, null)
        };
    
    private record ColourPairing(FlagColour Colour1, FlagColour Colour2);
}