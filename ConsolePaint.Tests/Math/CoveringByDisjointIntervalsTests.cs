using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ConsolePaint.Math;
using FluentAssertions;
using NUnit.Framework;

namespace ConsolePaint.Tests.Math;

[TestFixture]
public class CoveringByDisjointIntervalsTests
{
    private static IEnumerable<Func<CoveringByDisjointIntervals<double>>>
        ConstructionGuardsAgainstNullIntervalsTestCaseSource
        => new Func<CoveringByDisjointIntervals<double>>[]
        {
            () => new(null!),
            () => new((null as IEnumerable<(double, double)>)!)
        };

    [TestCaseSource(nameof(ConstructionGuardsAgainstNullIntervalsTestCaseSource))]
    public void Construction_guards_against_null_intervals(
        Func<CoveringByDisjointIntervals<double>> construction)
    {
        construction.Should().Throw<ArgumentNullException>().WithMessage("*intervals*");
    }

    private static IEnumerable<Func<CoveringByDisjointIntervals<double>>>
        ConstructionGuardsAgainstEmptyIntervalsTestCaseSource
        => new Func<CoveringByDisjointIntervals<double>>[]
        {
            () => new(),
            () => new(Enumerable.Empty<(double, double)>())
        };

    [TestCaseSource(nameof(ConstructionGuardsAgainstEmptyIntervalsTestCaseSource))]
    public void Construction_guards_against_empty_intervals(
        Func<CoveringByDisjointIntervals<double>> construction)
    {
        construction.Should().Throw<ArgumentException>().WithMessage("*intervals*");
    }

    [Test]
    public void Construction_guards_against_non_continuous_intervals()
    {
        var construction = () => new CoveringByDisjointIntervals<double>(
            (0, 1), (2, 3));

        construction.Should().Throw<InvalidOperationException>()
            .WithMessage("*discontinuous intervals*");
    }

    private static IEnumerable<Func<CoveringByDisjointIntervals<double>>> Constructors(
        params (double, double)[] intervals)
        => new Func<CoveringByDisjointIntervals<double>>[]
        {
            () => new(intervals),
            () => new(intervals.ToImmutableArray())
        };

    private static ImmutableArray<IEnumerable<(double Start, double End)>> IntervalsWithStartGreaterThanOrEqualToEnd
        => new[]
            {
                new (double Start, double End)[] { (0, 0) },
                new (double Start, double End)[] { (0, -1) },
                new (double Start, double End)[] { (0, 1), (1, 0) }
            }
            .Select(array => array.AsEnumerable())
            .ToImmutableArray();

    private static IEnumerable<Func<CoveringByDisjointIntervals<double>>>
        ConstructionGuardsAgainstIntervalsWithStartGreaterThanOrEqualToEndTestCaseSource
        => IntervalsWithStartGreaterThanOrEqualToEnd
            .SelectMany(intervals => Constructors(intervals.ToArray()));

    [TestCaseSource(nameof(ConstructionGuardsAgainstIntervalsWithStartGreaterThanOrEqualToEndTestCaseSource))]
    public void Construction_guards_against_intervals_with_start_greater_than_or_equal_to_end(
        Func<CoveringByDisjointIntervals<double>> construction)
    {
        construction.Should().Throw<InvalidOperationException>()
            .WithMessage("*intervals with start lower than or equal to end*");
    }

    [Test]
    public void Start_of_Covering_is_First_Interval_Start()
    {
        var covering = new CoveringByDisjointIntervals<double>((0, 1), (1, 2));
        covering.Start.Should().Be(0d);
    }

    [Test]
    public void End_of_Covering_is_Last_Interval_End()
    {
        var covering = new CoveringByDisjointIntervals<double>((0, 1), (1, 2));
        covering.End.Should().Be(2d);
    }

    [Test]
    public void Construction_orders_intervals()
    {
        var covering = new CoveringByDisjointIntervals<double>(
            (3, 4), (1, 2), (0, 1), (2, 3));

        covering.GetCover(0).Index.Should().Be(0);
        covering.GetCover(1).Index.Should().Be(1);
        covering.GetCover(2).Index.Should().Be(2);
        covering.GetCover(3).Index.Should().Be(3);
    }

    [TestCase(-1)]
    [TestCase(2)]
    public void GetCover_guards_against_argument_out_of_range(double element)
    {
        var covering = new CoveringByDisjointIntervals<double>((0, 1));

        var getCover = () => covering.GetCover(element);
        getCover.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*element*");
    }

    [Test]
    public void GetCover_works_with_right_open_intervals()
    {
        var covering = new CoveringByDisjointIntervals<double>((0, 1), (1, 2));

        var coverA = covering.GetCover(0.5);
        coverA.Interval.Start.Should().Be(0d);
        coverA.Interval.End.Should().Be(1d);
        coverA.Index.Should().Be(0);

        var coverB = covering.GetCover(1);
        coverB.Interval.Start.Should().Be(1d);
        coverB.Interval.End.Should().Be(2d);
        coverB.Index.Should().Be(1);
    }

    [Test]
    public void FromIntervalLengths_constructs_intervals_from_provided_lengths()
    {
        var covering = CoveringByDisjointIntervals<double>.FromIntervalLengths(
            0,
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