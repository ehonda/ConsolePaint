using System;
using System.Collections.Generic;
using System.Linq;
using ConsolePaint.Utility;
using FluentAssertions;
using NUnit.Framework;

namespace ConsolePaint.Tests.Utility;

public class EnumerateTests
{
    private int _nextEnumeratedSingleValue;

    private IEnumerable<int> DifferentSingleValueOnEachEnumeration
    {
        get { yield return _nextEnumeratedSingleValue++; }
    }

    [SetUp]
    public void SetUp()
    {
        _nextEnumeratedSingleValue = 0;
    }

    [Test]
    public void AndApplyGuardingAgainstNull_guards_against_null()
    {
        var transformation = () => Enumerate.AndApplyGuardingAgainstNull(
            null! as IEnumerable<int>,
            _ => 1);

        transformation.Should().Throw<ArgumentException>().WithMessage("*elements*");
    }

    [Test]
    public void AndApplyGuardingAgainstNull_enumerates_elements_exactly_once()
    {
        var expectedSingleValue = _nextEnumeratedSingleValue;

        Enumerate.AndApplyGuardingAgainstNull(
                DifferentSingleValueOnEachEnumeration,
                xs => xs.Single())
            .Should().Be(expectedSingleValue);
    }

    [Test]
    public void AndApplyGuardingAgainstNullOrEmpty_guards_against_null()
    {
        var transformation = () => Enumerate.AndApplyGuardingAgainstNullOrEmpty(
            null! as IEnumerable<int>,
            _ => 1);

        transformation.Should().Throw<ArgumentException>().WithMessage("*elements*");
    }

    [Test]
    public void AndApplyGuardingAgainstNullOrEmpty_guards_against_empty()
    {
        var transformation = () => Enumerate.AndApplyGuardingAgainstNullOrEmpty(
            Enumerable.Empty<int>(),
            _ => 1);

        transformation.Should().Throw<ArgumentException>().WithMessage("*elements*");
    }

    [Test]
    public void AndApplyGuardingAgainstNullOrEmpty_enumerates_elements_exactly_once()
    {
        var expectedSingleValue = _nextEnumeratedSingleValue;

        Enumerate.AndApplyGuardingAgainstNullOrEmpty(
                DifferentSingleValueOnEachEnumeration,
                xs => xs.Single())
            .Should().Be(expectedSingleValue);
    }
}