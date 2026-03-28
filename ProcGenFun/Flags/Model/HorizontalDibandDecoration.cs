namespace ProcGenFun.Flags.Model;

using Dunet;

[Union]
public partial record HorizontalDibandDecoration
{
    public partial record None;
    public partial record Fimbriation(FlagColour Colour);
    
    public enum Type
    {
        None,
        Fimbriation
    }
}