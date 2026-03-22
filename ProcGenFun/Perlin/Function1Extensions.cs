namespace ProcGenFun.Perlin;

public static class Function1Extensions
{
    public static IFunction1 Plus(this IFunction1 left, IFunction1 right) =>
        new CombineFunction(left, right, (l, r) => l + r);

    private class CombineFunction(IFunction1 left, IFunction1 right, Func<double, double, double> combine) : IFunction1
    {
        public double Evaluate(double x) => combine(left.Evaluate(x), right.Evaluate(x));
    }
}