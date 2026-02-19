namespace ProcGenFun.Flags;

using Dunet;

[Union]
public partial record FlagCharge
{
    public partial record None();
    public partial record Star(FlagColour Colour);
    public partial record StarBand(FlagColour Colour, int Count);
    public partial record Circle(FlagColour Colour);
    
    public enum Type { None, Star, StarBand, Circle }
}