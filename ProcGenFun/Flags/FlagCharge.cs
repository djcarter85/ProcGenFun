namespace ProcGenFun.Flags;

public abstract record FlagCharge
{
    private FlagCharge() { }

    public sealed record None() : FlagCharge;

    public sealed record Star(FlagColour Colour) : FlagCharge;

    public sealed record StarBand(FlagColour Colour) : FlagCharge;

    public enum Type { None, Star, StarBand }
}