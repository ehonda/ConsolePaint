namespace ConsolePaint.Demos;

// TODO: Better name
public static class Generate
{
    public static IEnumerable<(int X, int Y)> FlattenedGrid(int xLimit, int yLimit)
        => Enumerable
            .Repeat(Enumerable.Range(0, xLimit), yLimit)
            .Zip(Enumerable.Range(0, yLimit))
            .SelectMany(tuple => tuple.First.Select(x => (x, tuple.Second)));
}