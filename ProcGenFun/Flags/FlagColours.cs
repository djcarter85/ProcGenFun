namespace ProcGenFun.Flags;

using ProcGenFun.Distributions;
using RandN;

public static class FlagColours
{
    private static readonly IEnumerable<FlagColour> all =
    [
        FlagColour.Red,
        FlagColour.Orange,
        FlagColour.Yellow,
        FlagColour.Green,
        FlagColour.LightBlue,
        FlagColour.DarkBlue,
        FlagColour.Burgundy,
        FlagColour.Purple,
        FlagColour.Grey,
        FlagColour.White,
        FlagColour.Black
    ];

    public static IDistribution<FlagColour> AllDist() => ColourDist(all);

    public static IDistribution<FlagColour> AllowedAdjacentToDist(FlagColour adjacentColour) => 
        ColourDist(AllowedAdjacentTo(adjacentColour));

    private static IDistribution<FlagColour> ColourDist(IEnumerable<FlagColour> colours) =>
        WeightedDiscreteDistribution.New(colours.Select(c => new Weighting<FlagColour>(c, GetWeighting(c))));

    private static IEnumerable<FlagColour> AllowedAdjacentTo(FlagColour adjacentColour) => 
        all.Where(c => c != adjacentColour);

    private static int GetWeighting(FlagColour colour) =>
        colour switch
        {
            FlagColour.Red => 10,
            FlagColour.Orange => 3,
            FlagColour.Yellow => 6,
            FlagColour.Green => 10,
            FlagColour.LightBlue => 5,
            FlagColour.DarkBlue => 10,
            FlagColour.Burgundy => 2,
            FlagColour.Purple => 2,
            FlagColour.Grey => 1,
            FlagColour.White => 7,
            FlagColour.Black => 3,
            _ => throw new ArgumentOutOfRangeException(nameof(colour), colour, null)
        };
}