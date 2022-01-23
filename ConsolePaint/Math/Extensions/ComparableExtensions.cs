namespace ConsolePaint.Math.Extensions;

// TODO: Move to some Math nuget
public static class ComparableExtensions
{
    public static bool IsGreaterThan<TKey>(this TKey left, TKey right)
        where TKey : IComparable<TKey>
        => left.CompareTo(right) == 1;

    public static bool IsLowerThan<TKey>(this TKey left, TKey right)
        where TKey : IComparable<TKey>
        => left.CompareTo(right) == -1;

    public static bool IsGreaterThanOrEqualTo<TKey>(this TKey left, TKey right)
        where TKey : IComparable<TKey>
        => left.CompareTo(right) >= 0;

    public static bool IsLowerThanOrEqualTo<TKey>(this TKey left, TKey right)
        where TKey : IComparable<TKey>
        => left.CompareTo(right) <= 0;

    public static bool IsEqualTo<TKey>(this TKey left, TKey right)
        where TKey : IComparable<TKey>
        => left.CompareTo(right) == 0;
}