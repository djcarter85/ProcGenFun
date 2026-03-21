namespace ProcGenFun.Maps;

public readonly struct Vector2
{
    private readonly float radius;

    private Vector2(float radius, float thetaRadians)
    {
        this.radius = radius;
        this.ThetaRadians = thetaRadians;
    }
    
    public float ThetaRadians { get; }
    
    public float X => radius * MathF.Cos(ThetaRadians);
    
    public float Y => radius * MathF.Sin(ThetaRadians);

    public static Vector2 Unit(float theta) => new(radius: 1, thetaRadians: theta);

    public static Vector2 FromXY(float x, float y) =>
        new(radius: MathF.Sqrt(x * x + y * y), thetaRadians: MathF.Atan2(y, x));
}