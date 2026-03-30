namespace ProcGenFun.Flags.Model;

using Dunet;

[Union]
public partial record FlagChargeShape
{
    public partial record Star(FlagColour Colour);
    public partial record StarBand(FlagColour Colour, int Count);
    public partial record Circle(FlagColour Colour);
    public partial record Plus(FlagColour Colour);
    public partial record Shield(FlagColour Colour);
    
    public enum Type { Star, StarBand, Circle, Plus, Shield }
}