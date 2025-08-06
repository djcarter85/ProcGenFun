namespace ProcGenFun;

using NodaTime;
using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class Birthdays
{
    public static bool ThreePeopleShareBirthdayOnCamp(
        IEnumerable<LocalDate> birthdays) =>
        birthdays
            .Where(b =>
                b >= new LocalDate(2025, 07, 26) &&
                b <= new LocalDate(2025, 08, 03))
            .GroupBy(b => b)
            .Any(g => g.Count() >= 3);

    public static IDistribution<IEnumerable<LocalDate>> BirthdaySetDist() =>
        BirthdayDist().Repeat(71);

    public static IDistribution<LocalDate> BirthdayDist() =>
        from offset in Uniform.New(0, 365)
        select new LocalDate(2025, 01, 01).PlusDays(offset);
}