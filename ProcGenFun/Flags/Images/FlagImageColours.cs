namespace ProcGenFun.Flags.Images;

using System.Drawing;
using ProcGenFun.Flags.Model;

public static class FlagImageColours
{
    public static Color GetColor(FlagColour colour) =>
        colour switch
        {
            FlagColour.White => Color.FromArgb(255, 255, 255),
            FlagColour.Red => Color.FromArgb(200, 16, 46),
            FlagColour.DarkBlue => Color.FromArgb(0, 51, 160),
            FlagColour.RoyalBlue => Color.FromArgb(6, 102, 219),
            FlagColour.DarkGreen => Color.FromArgb(0, 102, 56),
            FlagColour.Gold => Color.FromArgb(252, 209, 22),
            FlagColour.Black => Color.FromArgb(0, 0, 0),
            FlagColour.LightGreen => Color.FromArgb(0, 153, 28),
            FlagColour.LightBlue => Color.FromArgb(108, 207, 246),
            FlagColour.Maroon => Color.FromArgb(122, 38, 58),
            FlagColour.Orange => Color.FromArgb(255, 130, 0),
            FlagColour.Purple => Color.FromArgb(102, 0, 153),
            _ => throw new ArgumentOutOfRangeException(nameof(colour), colour, null)
        };
}