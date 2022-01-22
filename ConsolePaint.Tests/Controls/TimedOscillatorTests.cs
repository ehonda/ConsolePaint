using System;
using System.Collections.Generic;
using ConsolePaint.Controls;
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
            TimeSpan.FromTicks(3),
        };

    [TestCaseSource(nameof(ElapsedTimeIsTooSmallTestCaseSource))]
    public void Oscillator_stays_in_the_same_state_if_elapsed_time_is_too_small(TimeSpan elapsed)
    {
        var stateA = new TimedState
        {
            LastsFor = TimeSpan.FromTicks(3)
        };
        var stateB = new TimedState
        {
            LastsFor = TimeSpan.FromTicks(3)
        };
        
        var oscillator = new TimedOscillator(new[]
        {
            stateA,
            stateB
        });

        oscillator.GetCurrentState(elapsed).Should().Be(stateA);
    }

    [Test]
    public void Oscillator_flips_from_one_state_to_the_other_cyclically()
    {
        var stateA = new TimedState
        {
            LastsFor = TimeSpan.FromTicks(3)
        };
        var stateB = new TimedState
        {
            LastsFor = TimeSpan.FromTicks(3)
        };
        
        var oscillator = new TimedOscillator(new[]
        {
            stateA,
            stateB
        });

        oscillator.GetCurrentState(TimeSpan.FromTicks(0)).Should().Be(stateA);
        oscillator.GetCurrentState(TimeSpan.FromTicks(4)).Should().Be(stateB);
        oscillator.GetCurrentState(TimeSpan.FromTicks(4)).Should().Be(stateA);
        oscillator.GetCurrentState(TimeSpan.FromTicks(4)).Should().Be(stateB);
    }
}