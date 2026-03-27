namespace ProcGenFun.Flags.Images;

using System.Drawing;
using ProcGenFun.Flags.Model;
using Svg;
using Svg.Pathing;
using Svg.Transforms;

public static class FlagImageCharges
{
    public static IEnumerable<SvgElement> GetChargesElements(IEnumerable<FlagCharge> charges) =>
        charges.Select(GetChargeElement);

    private static SvgElement GetChargeElement(FlagCharge charge)
    {
        var chargeElement = charge.Shape switch
        {
            FlagChargeShape.Star(var colour) => GetStarElement(colour, radius: GetRadius(charge.Size)),
            FlagChargeShape.StarBand(var colour, var count) => GetStarBandElement(colour, count, radius: GetRadius(charge.Size)),
            FlagChargeShape.Circle(var colour) => GetCircleElement(colour, radius: GetRadius(charge.Size)),
            FlagChargeShape.Plus(var colour) => GetPlusElement(colour, radius: GetRadius(charge.Size)),
            _ => throw new ArgumentOutOfRangeException(),
        };

        chargeElement.Transforms =
        [
            new SvgTranslate(
                x: GetChargeCentreX(charge.Location),
                y: GetChargeCentreY(charge.Location))
        ];
        
        return chargeElement;
    }

    private static float GetRadius(FlagChargeSize size) =>
        size switch
        {
            FlagChargeSize.Small => 1.5f * FlagImageSizing.U,
            FlagChargeSize.Medium => 2 * FlagImageSizing.U,
            FlagChargeSize.Large => 3 * FlagImageSizing.U,
            _ => throw new ArgumentOutOfRangeException(nameof(size), size, null)
        };

    private static float GetChargeCentreX(FlagChargeLocation chargeLocation) =>
        chargeLocation switch
        {
            FlagChargeLocation.TopLeft or FlagChargeLocation.CentreLeft => 4.5f * FlagImageSizing.U,
            FlagChargeLocation.Centre => 9 * FlagImageSizing.U,
            FlagChargeLocation.CentreRight => 13.5f * FlagImageSizing.U,
            _ => throw new ArgumentOutOfRangeException(nameof(chargeLocation), chargeLocation, null)
        };

    private static int GetChargeCentreY(FlagChargeLocation chargeLocation) =>
        chargeLocation switch
        {
            FlagChargeLocation.TopLeft => 3 * FlagImageSizing.U,
            FlagChargeLocation.CentreLeft or FlagChargeLocation.Centre or FlagChargeLocation.CentreRight => 6 * FlagImageSizing.U,
            _ => throw new ArgumentOutOfRangeException(nameof(chargeLocation), chargeLocation, null)
        };

    private static SvgElement GetStarElement(FlagColour colour, float radius) =>
        CreateSvgStar(
            centre: PointF.Empty,
            radius: radius,
            fillColour: FlagImageColours.GetColor(colour));

    private static SvgElement GetStarBandElement(FlagColour colour, int count, float radius)
    {
        var groupElement = new SvgGroup();
        
        var distanceBetweenCentres = 2.5f * radius;
        var firstCentreX = -(count - 1) / 2f * distanceBetweenCentres;
        for (int i = 0; i < count; i++)
        {
            groupElement.Children.Add(CreateSvgStar(
                    centre: new PointF(firstCentreX + i * distanceBetweenCentres, 0),
                    radius: radius,
                    fillColour: FlagImageColours.GetColor(colour)));
        }

        return groupElement;
    }

    private static SvgElement GetCircleElement(FlagColour colour, float radius) =>
        new SvgCircle
            { CenterX = 0, CenterY = 0, Radius = radius, Fill = new SvgColourServer(FlagImageColours.GetColor(colour)) };

    private static SvgElement GetPlusElement(FlagColour colour, float radius) =>
        new SvgPath
        {
            Stroke = new SvgColourServer(FlagImageColours.GetColor(colour)),
            StrokeWidth = 0.5f * radius,
            PathData = [
                new SvgMoveToSegment(false, new PointF(0, -radius)),
                new SvgLineSegment(false, new PointF(0, radius)),
                new SvgMoveToSegment(false, new PointF(-radius, 0)),
                new SvgLineSegment(false, new PointF(radius, 0)),
            ]
        };

    private static SvgPath CreateSvgStar(PointF centre, float radius, Color fillColour) =>
        new()
        {
            PathData = SvgExtensions.ToPathData(ClosedPath([
                RadialPoint(radius, -0.5f * MathF.PI), RadialPoint(radius, 0.3f * MathF.PI), RadialPoint(radius, -0.9f * MathF.PI), RadialPoint(radius, -0.1f * MathF.PI), RadialPoint(radius, 0.7f * MathF.PI),
            ])),
            Fill = new SvgColourServer(fillColour),
            Transforms =
            [
                new SvgTranslate(centre.X, centre.Y)
            ]
        };

    private static PointF RadialPoint(float radius, float angle) =>
        new(x: radius * MathF.Cos(angle), y: radius * MathF.Sin(angle));

    private static IEnumerable<SvgPathSegment> ClosedPath(IEnumerable<PointF> points)
    {
        var isFirst = true;
        foreach (var point in points)
        {
            if (isFirst)
            {
                yield return new SvgMoveToSegment(false, point);
            }
            else
            {
                yield return new SvgLineSegment(false, point);
            }

            isFirst = false;
        }
    }
}