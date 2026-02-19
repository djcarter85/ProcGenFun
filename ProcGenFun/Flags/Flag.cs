namespace ProcGenFun.Flags;

using Dunet;

[Union]
public partial record Flag
{
    public partial record Solid(FlagColour Colour, FlagCharge Charge);
    public partial record VerticalDiband(FlagColour Left, FlagColour Right);
    public partial record HorizontalDiband(FlagColour Top, FlagColour Bottom);
    public partial record VerticalTriband(FlagColour Left, FlagColour Middle, FlagColour Right, FlagCharge Charge);
    public partial record HorizontalTriband(FlagColour Top, FlagColour Middle, FlagColour Bottom, FlagCharge Charge);
    public partial record Cross(FlagColour Background, FlagColour Foreground, CrossType CrossType);
    public partial record Saltire(FlagColour Background, FlagColour Foreground);

    public enum Type { Solid, VerticalDiband, HorizontalDiband, VerticalTriband, HorizontalTriband, Cross, Saltire }
}

public enum CrossType { Regular, Nordic }