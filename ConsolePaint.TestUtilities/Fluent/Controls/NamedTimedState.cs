using ConsolePaint.TestUtilities.Controls;
using JetBrains.Annotations;

namespace ConsolePaint.TestUtilities.Fluent.Controls;

[PublicAPI]
public static class NamedTimedState
{
    public static NamedTimedStateBuilder Lasting(long ticks)
        => new NamedTimedStateBuilder().Lasting(ticks);

    public static NamedTimedStateBuilder WithName(string name)
        => new NamedTimedStateBuilder().WithName(name);
}