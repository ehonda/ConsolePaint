namespace ConsolePaint.Controls;

// TODO: Better name!
public class EnumValuedTimedState<TEnum> : TimedState where TEnum : Enum
{
    // TODO: Can we init with default! and get rid of the constructor?
    public TEnum Value { get; }
    
    public EnumValuedTimedState(TimeSpan lastsFor, TEnum value)
    {
        LastsFor = lastsFor;
        Value = value;
    }
}