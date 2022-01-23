using JetBrains.Annotations;

namespace ConsolePaint.TestUtilities.Controls;

[PublicAPI]
public class NamedTimedStateBuilder
{
    private TimeSpan _lastsFor = TimeSpan.Zero;
    private string _name = string.Empty;

    public NamedTimedStateBuilder Lasting(long ticks)
    {
        _lastsFor = TimeSpan.FromTicks(ticks);
        return this;
    }

    public NamedTimedStateBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public NamedTimedState Create() => new()
    {
        LastsFor = _lastsFor,
        Name = _name
    };
}