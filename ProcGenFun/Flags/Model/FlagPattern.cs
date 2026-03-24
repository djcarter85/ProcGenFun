namespace ProcGenFun.Flags.Model;

using Dunet;

[Union]
public partial record FlagPattern
{
    public partial record Solid(FlagColour Colour);
    public partial record Canton(FlagColour Field, FlagColour CantonColour);
    public partial record VerticalDiband(FlagColour Left, FlagColour Right);
    public partial record HorizontalDiband(FlagColour Top, FlagColour Bottom, FlagColour? Fimbriation);
    public partial record VerticalTriband(FlagColour Left, FlagColour Middle, FlagColour Right);
    public partial record HorizontalTriband(FlagColour Top, FlagColour Middle, FlagColour Bottom);
    public partial record DiagonalBicolour(FlagColour Left, FlagColour Right, Diagonal Diagonal);
    public partial record Cross(FlagColour Background, FlagColour Foreground, CrossType CrossType);
    public partial record Saltire(FlagColour NorthSouthField, FlagColour EastWestField, FlagColour Foreground);
    public partial record Quartered(FlagColour TopLeft, FlagColour TopRight, FlagColour BottomRight, FlagColour BottomLeft);
    public partial record HorizontalStriped(FlagColour Colour1, FlagColour Colour2, int StripeCount);

    public enum Type
    {
        Solid,
        Canton,
        VerticalDiband,
        HorizontalDiband,
        VerticalTriband,
        HorizontalTriband,
        DiagonalBicolour,
        Cross,
        Saltire,
        Quartered,
        HorizontalStriped
    }
}