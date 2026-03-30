namespace ProcGenFun.Flags.Model;

using Dunet;

[Union]
public partial record DiagonalBicolourDecoration
{
    public partial record None;
    public partial record LeftRay(FlagColour Colour);
    public partial record RightRay(FlagColour Colour);
}