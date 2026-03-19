namespace ProcGenFun.Maps;

public readonly struct Vector2
{
    private Vector2(float thetaRadians)
    {
        this.ThetaRadians = thetaRadians;
    }
    
    public float ThetaRadians { get; }

    public static Vector2 Unit(float theta) => new(thetaRadians: theta);
}