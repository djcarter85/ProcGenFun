namespace ProcGenFun;

public class Histogram
{
    private Histogram(IReadOnlyList<HistogramBucket> buckets, double bucketWidth)
    {
        this.Buckets = buckets;
        this.BucketWidth = bucketWidth;
    }

    public IReadOnlyList<HistogramBucket> Buckets { get; }

    public double BucketWidth { get; }

    public static Histogram New(IEnumerable<double> values, double min, double max)
    {
        const int bucketCount = 100;
        var buckets = new int[bucketCount];

        foreach (var value in values)
        {
            var bucketIndex = (int)(bucketCount * (value - min) / (max - min));
            if (0 <= bucketIndex && bucketIndex < bucketCount)
            {
                buckets[bucketIndex]++;
            }
        }

        var bucketWidth = (max - min) / bucketCount;

        return new Histogram(
            buckets
                .Select((count, index) => new HistogramBucket(
                    Centre: min + bucketWidth * (index + 0.5),
                    Count: count))
                .ToList(),
            bucketWidth);
    }
}
