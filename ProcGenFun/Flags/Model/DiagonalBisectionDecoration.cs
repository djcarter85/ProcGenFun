namespace ProcGenFun.Flags.Model;

using Dunet;

[Union]
public partial record DiagonalBisectionDecoration
{
    public partial record None;
    public partial record LeftRay(FlagColour Colour);
    public partial record RightRay(FlagColour Colour);
}