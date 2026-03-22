namespace ProcGenFun.Perlin;

public static class Function1
{
    private static IFunction1 Zero => new ZeroFunction();

    public static IEnumerable<IFunction1> Accumulate(IEnumerable<IFunction1> function1s)
    {
        var cumulative = Function1.Zero;
        foreach (var function1 in function1s)
        {
            cumulative = cumulative.Plus(function1);
            yield return cumulative;
        }
    }
    
    private class ZeroFunction : IFunction1
    {
        public double Evaluate(double x) => 0;
    }
}