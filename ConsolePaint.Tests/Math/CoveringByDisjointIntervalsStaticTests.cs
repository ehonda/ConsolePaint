using System;
using System.Collections.Generic;
using ConsolePaint.Math;
using FluentAssertions;
using NUnit.Framework;

namespace ConsolePaint.Tests.Math;

[TestFixture]
public class CoveringByDisjointIntervalsStaticTests
{
    [Test]
    public void FromIntervalLengths_guards_against_null_addElements()
    {
        var fromIntervalLengths = () => CoveringByDisjointIntervals.FromIntervalLengths(
            0d,
            null!,
            1d);

        fromIntervalLengths.Should().Throw<ArgumentException>().WithMessage("*addElements*");
    }
    
    // TODO: Test params version as well
    [Test]
    public void FromIntervalLengths_guards_against_null_lengths()
    {
        var fromIntervalLengths = () => CoveringByDisjointIntervals.FromIntervalLengths(
            0d,
            (a, b) => a + b,
            null! as IEnumerable<double>);

        fromIntervalLengths.Should().Throw<ArgumentException>().WithMessage("*lengths*");
    }
    
    [Test]
    public void FromIntervalLengths_constructs_intervals_from_provided_lengths()
    {
        var covering = CoveringByDisjointIntervals.FromIntervalLengths(
            0d,
            (a, b) => a + b,
            1, 2, 3);

        var coverA = covering.GetCover(0);
        coverA.Interval.Start.Should().Be(0d);
        coverA.Interval.End.Should().Be(1d);
        coverA.Index.Should().Be(0);

        var coverB = covering.GetCover(1);
        coverB.Interval.Start.Should().Be(1d);
        coverB.Interval.End.Should().Be(3d);
        coverB.Index.Should().Be(1);

        var coverC = covering.GetCover(3);
        coverC.Interval.Start.Should().Be(3d);
        coverC.Interval.End.Should().Be(6d);
        coverC.Index.Should().Be(2);
    }
}