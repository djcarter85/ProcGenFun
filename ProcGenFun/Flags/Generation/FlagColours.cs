namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Distributions;
using ProcGenFun.Flags.Model;
using RandN;

public static class FlagColours
{
    private static readonly IEnumerable<FlagColour> all = Enum.GetValues<FlagColour>();

    private static readonly IEnumerable<ColourPairing> disallowedColourPairings = [];

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
            FlagColour.Green => 70,
            FlagColour.Yellow => 70,
            FlagColour.Black => 65,
            FlagColour.LightBlue => 40,
            FlagColour.Orange => 25,
            FlagColour.Brown => 10,
            FlagColour.Purple => 2,
            _ => throw new ArgumentOutOfRangeException(nameof(colour), colour, null)
        };
    
    private record ColourPairing(FlagColour Colour1, FlagColour Colour2);
}