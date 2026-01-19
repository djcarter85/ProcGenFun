namespace ProcGenFun.Flags;

public abstract record Flag
{
    private Flag() { }

    public sealed record Solid(FlagColour Colour) : Flag;

    public sealed record VerticalDiband(FlagColour Left, FlagColour Right) : Flag;
    
    public sealed record HorizontalDiband(FlagColour Top, FlagColour Bottom) : Flag;

    public sealed record VerticalTriband(FlagColour Left, FlagColour Middle, FlagColour Right) : Flag;
    
    public sealed record HorizontalTriband(FlagColour Top, FlagColour Middle, FlagColour Bottom) : Flag;
    
    public sealed record Cross(FlagColour Background, FlagColour Foreground) : Flag;

    public enum Type { Solid, VerticalDiband, HorizontalDiband, VerticalTriband, HorizontalTriband, Cross }
}
