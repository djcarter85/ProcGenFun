namespace ProcGenFun;

using Svg.Pathing;

public static class SvgExtensions
{
    public static SvgPathSegmentList ToPathData(this IEnumerable<SvgPathSegment> pathSegments)
    {
        var pathData = new SvgPathSegmentList();
        foreach (var segment in pathSegments)
        {
            pathData.Add(segment);
        }
        return pathData;
    }
}