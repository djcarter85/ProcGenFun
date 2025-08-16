namespace ProcGenFun;

using System.Globalization;
using CsvHelper;
using NodaTime;
using NodaTime.Text;
using ProcGenFun.Distributions;
using RandN;

public static class Birthdays
{
    public static bool PeopleShareBirthday(
        IEnumerable<AnnualDate> birthdays,
        int sharedBirthdayCount,
        bool sharedBirthdayMustOccurDuringCamp)
    {
        if (sharedBirthdayMustOccurDuringCamp)
        {
            birthdays = birthdays
                .Where(b =>
                    b >= new AnnualDate(07, 26) &&
                    b <= new AnnualDate(08, 03));
        }

        return birthdays
            .GroupBy(b => b)
            .Any(g => g.Count() >= sharedBirthdayCount);
    }

    public static IDistribution<IEnumerable<AnnualDate>> BirthdaySetDist(int size) =>
        BirthdayDist().Repeat(size);

    public static IDistribution<AnnualDate> BirthdayDist() =>
        WeightedDiscreteDistribution.New(GetBirthdayWeights());

    private static IReadOnlyList<Weighting<AnnualDate>> GetBirthdayWeights()
    {
        using var streamReader = new StreamReader("BirthdayWeights.csv");
        using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

        var pattern = AnnualDatePattern.CreateWithInvariantCulture("MM-dd");

        return csvReader
            .GetRecords<CsvBirthdayWeight>()
            .Select(x => new Weighting<AnnualDate>(pattern.Parse(x.Date).GetValueOrThrow(), x.Weight))
            .ToList();
    }

    private class CsvBirthdayWeight
    {
        public string Date { get; set; } = null!;

        public int Weight { get; set; }
    }
}