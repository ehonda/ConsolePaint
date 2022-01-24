using JetBrains.Annotations;

namespace ConsolePaint.TestUtilities.Controls;

[PublicAPI]
public static partial class Fluent
{
    public static class NamedTimedState
    {
        public static NamedTimedStateBuilder Lasting(long ticks)
            => new NamedTimedStateBuilder().Lasting(ticks);

        public static NamedTimedStateBuilder WithName(string name)
            => new NamedTimedStateBuilder().WithName(name);
    }
}