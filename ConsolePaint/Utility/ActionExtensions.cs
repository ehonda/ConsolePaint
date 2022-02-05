namespace ConsolePaint.Utility;

public static class ActionExtensions
{
    // TODO: Better return type, ability to specify default, etc.
    // ReSharper disable once InconsistentNaming
    public static Func<T, S?> AsFunction<T, S>(this Action<T> action)
        => t =>
        {
            action(t);
            return default;
        };

    public static Func<T, int> AsFunction<T>(this Action<T> action)
        => action.AsFunction<T, int>();
}