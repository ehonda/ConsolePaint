using System;
using System.Collections.Generic;
using ConsolePaint.Controls;
using ConsolePaint.TestUtilities.Controls;
using FluentAssertions;
using NUnit.Framework;

namespace ConsolePaint.Tests.Controls;

[TestFixture]
public class TimedOscillatorTests
{
    // TODO: Test that at least one state is given
    // TODO: Test that elapsed is not negative
    // TODO: Tests for timed state timespan values
    // TODO: Nicer test fluent syntax (TimedState.LastingTicks(3))

    private static IEnumerable<TimeSpan> ElapsedTimeIsTooSmallTestCaseSource =>
        new[]
        {
            TimeSpan.FromTicks(0),
            TimeSpan.FromTicks(1),
            TimeSpan.FromTicks(2),
        };

    [TestCaseSource(nameof(ElapsedTimeIsTooSmallTestCaseSource))]
    public void Oscillator_stays_in_the_same_state_if_elapsed_time_is_too_small(TimeSpan elapsed)
    {
        var stateA = Fluent.NamedTimedState.Lasting(3).WithName("A").Create();
        var stateB = Fluent.NamedTimedState.Lasting(3).WithName("B").Create();
        
        var oscillator = new TimedOscillator<NamedTimedState>(stateA, stateB);

        oscillator.Step(elapsed).Should().Be(stateA);
    }

    [Test]
    public void Oscillator_flips_from_one_state_to_the_other_cyclically()
    {
        var stateA = Fluent.NamedTimedState.Lasting(3).WithName("A").Create();
        var stateB = Fluent.NamedTimedState.Lasting(3).WithName("B").Create();
        
        var oscillator = new TimedOscillator<NamedTimedState>(stateA, stateB);

        oscillator.Step(TimeSpan.FromTicks(0)).Should().Be(stateA);
        oscillator.Step(TimeSpan.FromTicks(4)).Should().Be(stateB);
        oscillator.Step(TimeSpan.FromTicks(4)).Should().Be(stateA);
        oscillator.Step(TimeSpan.FromTicks(3)).Should().Be(stateB);
    }
    
    [Test]
    public void Oscillator_skips_a_state()
    {
        var stateA = Fluent.NamedTimedState.Lasting(3).WithName("A").Create();
        var stateB = Fluent.NamedTimedState.Lasting(3).WithName("B").Create();
        var stateC = Fluent.NamedTimedState.Lasting(3).WithName("C").Create();
        
        var oscillator = new TimedOscillator<NamedTimedState>(stateA, stateB, stateC);

        oscillator.Step(TimeSpan.FromTicks(7)).Should().Be(stateC);
    }
    
    [Test]
    public void Oscillator_moves_a_full_period()
    {
        var stateA = Fluent.NamedTimedState.Lasting(3).WithName("A").Create();
        var stateB = Fluent.NamedTimedState.Lasting(3).WithName("B").Create();
        var stateC = Fluent.NamedTimedState.Lasting(3).WithName("C").Create();
        var stateD = Fluent.NamedTimedState.Lasting(3).WithName("D").Create();
        
        var oscillator = new TimedOscillator<NamedTimedState>(stateA, stateB, stateC, stateD);

        oscillator.Step(TimeSpan.FromTicks(19)).Should().Be(stateC);
    }
}