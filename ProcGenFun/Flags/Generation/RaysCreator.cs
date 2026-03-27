namespace ProcGenFun.Flags.Generation;

using ProcGenFun.Flags.Model;
using RandN;
using RandN.Extensions;

public static class RaysCreator
{
    public static IDistribution<Flag> Dist() =>
        from field in FlagColours.AllDist()
        from middle in FlagColours.AllowedAdjacentToDist((FlagColour)field)
        from foreground in FlagColours.AllowedAdjacentToDist(middle)
        select new Flag(new FlagPattern.Rays(field, middle, foreground), []);
}