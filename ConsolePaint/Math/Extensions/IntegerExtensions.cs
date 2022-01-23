namespace ConsolePaint.Math.Extensions;

public static class IntegerExtensions
{
    public static int AddModulo(this int a, int b, int n)
        => ((a + b) % n + n) % n;
}