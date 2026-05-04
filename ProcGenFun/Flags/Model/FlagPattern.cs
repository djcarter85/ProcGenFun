namespace ProcGenFun.Flags.Model;

using Dunet;

[Union]
public partial record FlagPattern
{
    public partial record Solid(FlagColour Field);
    public partial record Canton(FlagColour Field, FlagColour CantonColour);
    public partial record VerticalBisection(FlagColour Left, FlagColour Right);
    public partial record HorizontalBisection(FlagColour Top, FlagColour Bottom, HorizontalBisectionDecoration Decoration);
    public partial record VerticalTriband(FlagColour Left, FlagColour Middle, FlagColour Right);
    public partial record HorizontalTriband(FlagColour Top, FlagColour Middle, FlagColour Bottom, HorizontalTribandSizing Sizing, FlagColour? Fimbriation);
    public partial record DiagonalBisection(FlagColour Left, FlagColour Right, Diagonal Diagonal, DiagonalBisectionDecoration Decoration);
    public partial record Cross(FlagColour Field, FlagColour Foreground, CrossType CrossType);
    public partial record Saltire(FlagColour NorthSouthField, FlagColour EastWestField, FlagColour Foreground, FlagColour? Fimbriation);
    public partial record Quadrisection(FlagColour TopLeft, FlagColour TopRight, FlagColour BottomRight, FlagColour BottomLeft);
    public partial record HorizontalStriped(FlagColour Colour1, FlagColour Colour2, int StripeCount);
    public partial record Pall(FlagColour Field, FlagColour Foreground, FlagColour? Fimbriation);
}