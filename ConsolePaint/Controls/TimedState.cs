namespace ConsolePaint.Controls;

// TODO: To better namespace
public class TimedState
{
    public TimeSpan LastsFor { get; init; } = TimeSpan.Zero;

    public TimeSpan Elapsed { get; set; } = TimeSpan.Zero;
}