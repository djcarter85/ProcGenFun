namespace ProcGenFun.Flags;

public abstract record Flag
{
    private Flag() { }

    public sealed record Solid(FlagColour Colour) : Flag;

    public static IEnumerable<Type> Types { get; } = [Type.Solid];

    public enum Type { Solid }
}
