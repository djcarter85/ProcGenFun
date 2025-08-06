namespace ProcGenFun;

using NodaTime;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class Birthdays
{
    public static IDistribution<LocalDate> BirthdayDist() =>
        from offset in Uniform.New(0, 365)
        select new LocalDate(2025, 01, 01).PlusDays(offset);
}