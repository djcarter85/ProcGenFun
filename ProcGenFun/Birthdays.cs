namespace ProcGenFun;

using NodaTime;
using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class Birthdays
{
    public static bool PeopleShareBirthday(
        IEnumerable<LocalDate> birthdays, 
        int sharedBirthdayCount,
        bool sharedBirthdayMustOccurDuringCamp)
    {
        if (sharedBirthdayMustOccurDuringCamp)
        {
            birthdays = birthdays
                .Where(b =>
                    b >= new LocalDate(2025, 07, 26) &&
                    b <= new LocalDate(2025, 08, 03));
        }

        return birthdays
            .GroupBy(b => b)
            .Any(g => g.Count() >= sharedBirthdayCount);
    }

    public static IDistribution<IEnumerable<LocalDate>> BirthdaySetDist(int size) =>
        BirthdayDist().Repeat(size);

    public static IDistribution<LocalDate> BirthdayDist() =>
        from offset in Uniform.New(0, 365)
        select new LocalDate(2025, 01, 01).PlusDays(offset);
}