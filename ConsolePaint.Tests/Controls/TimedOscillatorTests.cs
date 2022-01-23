﻿using System;
using System.Collections.Generic;
using ConsolePaint.Controls;
using ConsolePaint.TestUtilities.Controls;
using FluentAssertions;
using NUnit.Framework;

namespace ConsolePaint.Tests.Controls;

// TODO: Better way to resolve name clashes
using FluentNamedTimedState = ConsolePaint.TestUtilities.Fluent.Controls.NamedTimedState;

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
        var stateA = FluentNamedTimedState.Lasting(3).WithName("A").Create();
        var stateB = FluentNamedTimedState.Lasting(3).WithName("B").Create();
        
        var oscillator = new TimedOscillator<NamedTimedState>(stateA, stateB);

        oscillator.GetCurrentState(elapsed).Should().Be(stateA);
    }

    [Test]
    public void Oscillator_flips_from_one_state_to_the_other_cyclically()
    {
        var stateA = FluentNamedTimedState.Lasting(3).WithName("A").Create();
        var stateB = FluentNamedTimedState.Lasting(3).WithName("B").Create();
        
        var oscillator = new TimedOscillator<NamedTimedState>(stateA, stateB);

        oscillator.GetCurrentState(TimeSpan.FromTicks(0)).Should().Be(stateA);
        oscillator.GetCurrentState(TimeSpan.FromTicks(4)).Should().Be(stateB);
        oscillator.GetCurrentState(TimeSpan.FromTicks(4)).Should().Be(stateA);
        oscillator.GetCurrentState(TimeSpan.FromTicks(3)).Should().Be(stateB);
    }
    
    [Test]
    public void Oscillator_skips_a_state()
    {
        var stateA = FluentNamedTimedState.Lasting(3).WithName("A").Create();
        var stateB = FluentNamedTimedState.Lasting(3).WithName("B").Create();
        var stateC = FluentNamedTimedState.Lasting(3).WithName("C").Create();
        
        var oscillator = new TimedOscillator<NamedTimedState>(stateA, stateB, stateC);

        oscillator.GetCurrentState(TimeSpan.FromTicks(7)).Should().Be(stateC);
    }
    
    [Test]
    public void Oscillator_moves_a_full_period()
    {
        var stateA = FluentNamedTimedState.Lasting(3).WithName("A").Create();
        var stateB = FluentNamedTimedState.Lasting(3).WithName("B").Create();
        var stateC = FluentNamedTimedState.Lasting(3).WithName("C").Create();
        var stateD = FluentNamedTimedState.Lasting(3).WithName("D").Create();
        
        var oscillator = new TimedOscillator<NamedTimedState>(stateA, stateB, stateC, stateD);

        oscillator.GetCurrentState(TimeSpan.FromTicks(19)).Should().Be(stateC);
    }
}