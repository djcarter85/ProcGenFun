namespace ProcGenFun.Flags.Images;

using System.Drawing;
using ProcGenFun.Flags.Model;
using Svg;
using Svg.Pathing;
using Svg.Transforms;
using static FlagImageSizing;
using static ProcGenFun.Flags.Model.FlagChargeShape;

public static class FlagImageCharges
{
    public static IEnumerable<SvgElement> GetChargesElements(IEnumerable<FlagCharge> charges) =>
        charges.Select(GetChargeElement);

    private static SvgElement GetChargeElement(FlagCharge charge)
    {
        var chargeElement = charge.Shape switch
        {
            Star star => GetStarElement(star, radius: GetRadius(charge.Size)),
            StarBand starBand => GetStarBandElement(starBand, radius: GetRadius(charge.Size)),
            Circle circle => GetCircleElement(circle, radius: GetRadius(charge.Size)),
            Crescent crescent => GetCrescentElement(crescent, radius: GetRadius(charge.Size)),
            Plus plus => GetPlusElement(plus, radius: GetRadius(charge.Size)),
            Shield shield => GetShieldElement(shield, radius: GetRadius(charge.Size)),
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
            FlagChargeSize.Small => 1.25f * U,
            FlagChargeSize.Medium => 2 * U,
            FlagChargeSize.Large => 2.5f * U,
            FlagChargeSize.ExtraLarge => 3 * U,
            _ => throw new ArgumentOutOfRangeException(nameof(size), size, null)
        };

    private static float GetChargeCentreX(FlagChargeLocation chargeLocation) =>
        chargeLocation switch
        {
            FlagChargeLocation.CentreLeftThird => 3f * U,
            FlagChargeLocation.TopHalfLeftHalf or FlagChargeLocation.CentreLeftHalf => 4.5f * U,
            FlagChargeLocation.Centre => 9 * U,
            FlagChargeLocation.CentreRightHalf => 13.5f * U,
            _ => throw new ArgumentOutOfRangeException(nameof(chargeLocation), chargeLocation, null)
        };

    private static int GetChargeCentreY(FlagChargeLocation chargeLocation) =>
        chargeLocation switch
        {
            FlagChargeLocation.TopHalfLeftHalf => 3 * U,
            FlagChargeLocation.CentreLeftThird or FlagChargeLocation.CentreLeftHalf or FlagChargeLocation.Centre or FlagChargeLocation.CentreRightHalf => 6 * U,
            _ => throw new ArgumentOutOfRangeException(nameof(chargeLocation), chargeLocation, null)
        };

    private static SvgElement GetStarElement(Star star, float radius) =>
        CreateSvgStar(
            centre: PointF.Empty,
            radius: radius,
            fillColour: FlagImageColours.GetColor(star.Colour));

    private static SvgElement GetStarBandElement(StarBand starBand, float radius)
    {
        var groupElement = new SvgGroup();
        
        var distanceBetweenCentres = 2.2f * radius;
        var firstCentreX = -(starBand.Count - 1) / 2f * distanceBetweenCentres;
        for (int i = 0; i < starBand.Count; i++)
        {
            groupElement.Children.Add(CreateSvgStar(
                    centre: new PointF(firstCentreX + i * distanceBetweenCentres, 0),
                    radius: radius,
                    fillColour: FlagImageColours.GetColor(starBand.Colour)));
        }

        return groupElement;
    }

    private static SvgElement GetCircleElement(Circle circle, float radius) =>
        new SvgCircle
            { CenterX = 0, CenterY = 0, Radius = radius, Fill = new SvgColourServer(FlagImageColours.GetColor(circle.Colour)) };

    private static SvgElement GetCrescentElement(Crescent crescent, float radius)
    {
        var startPoint = RadialPoint(radius, 0.2f * MathF.PI);
        var endPoint = RadialPoint(radius, -0.2f * MathF.PI);
        var outerRadius = radius;
        var innerRadius = 0.8f * radius;
        return new SvgPath
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(crescent.Colour)),
            PathData =
            [
                new SvgMoveToSegment(false, startPoint),
                new SvgArcSegment(outerRadius, outerRadius, angle: 0, SvgArcSize.Large, SvgArcSweep.Positive, false, endPoint),
                new SvgArcSegment(innerRadius, innerRadius, angle: 0, SvgArcSize.Large, SvgArcSweep.Negative, false, startPoint),
            ]
        };
    }

    private static SvgElement GetPlusElement(Plus plus, float radius) =>
        new SvgPath
        {
            Stroke = new SvgColourServer(FlagImageColours.GetColor(plus.Colour)),
            StrokeWidth = 0.5f * radius,
            PathData = [
                new SvgMoveToSegment(false, new PointF(0, -radius)),
                new SvgLineSegment(false, new PointF(0, radius)),
                new SvgMoveToSegment(false, new PointF(-radius, 0)),
                new SvgLineSegment(false, new PointF(radius, 0)),
            ]
        };

    private static SvgElement GetShieldElement(Shield shield, float radius)
    {
        var xRadius = 0.6f * radius;
        var yRadius = 0.8f * radius;
        
        return new SvgPath
        {
            Fill = new SvgColourServer(FlagImageColours.GetColor(shield.Colour)),
            PathData =
            [
                new SvgMoveToSegment(false, new PointF(-xRadius, -yRadius)),
                new SvgLineSegment(false, new PointF(xRadius, -yRadius)),
                new SvgLineSegment(false, new PointF(xRadius, 0)),
                new SvgArcSegment(xRadius, yRadius, 0, SvgArcSize.Small, SvgArcSweep.Positive, false, new PointF(-xRadius, 0)),
                new SvgClosePathSegment(false),
            ]
        };
    }

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