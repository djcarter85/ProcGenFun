using RandN;
using RandN.Distributions;

namespace ProcGenFun.Flags;

public static class FlagCreator
{
    public static IDistribution<Flag> FlagDist()
    {
        return Singleton.New(new Flag());
    }
}