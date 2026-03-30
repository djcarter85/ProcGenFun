namespace ProcGenFun.Flags.Model;

using Dunet;

[Union]
public partial record HorizontalDibandDecoration
{
    public partial record None;
    public partial record Fimbriation(FlagColour Colour);
    public partial record Pile(FlagColour Colour);
    public partial record VerticalBand(FlagColour Colour);
}