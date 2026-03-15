namespace ProcGenFun.Flags;

using Dunet;

[Union]
public partial record FlagCharge
{
    public partial record None;
    public partial record Star(FlagColour Colour, float Size);
    public partial record StarBand(FlagColour Colour, int Count, float Size);
    public partial record Circle(FlagColour Colour, float Size);
    
    public enum Type { None, Star, StarBand, Circle }
}