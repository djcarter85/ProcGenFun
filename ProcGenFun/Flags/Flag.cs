namespace ProcGenFun.Flags;

public abstract record Flag
{
    private Flag() { }

    public sealed record Solid(FlagColour Colour) : Flag;

    public sealed record VerticalDiband(FlagColour Left, FlagColour Right) : Flag;

    public sealed record VerticalTriband(FlagColour Left, FlagColour Middle, FlagColour Right) : Flag;

    public static IEnumerable<Type> Types { get; } = [Type.Solid, Type.VerticalDiband, Type.VerticalTriband];

    public enum Type { Solid, VerticalDiband, VerticalTriband }
}
