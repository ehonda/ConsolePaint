using System;
using System.Collections.Generic;
using System.Linq;
using ConsolePaint.Utility;
using FluentAssertions;
using NUnit.Framework;

namespace ConsolePaint.Tests.Utility;

public class WithTests
{
    private int _nextEnumeratedSingleValue;

    private IEnumerable<int> EnumeratesDifferentlyEachTime
    {
        get { yield return _nextEnumeratedSingleValue++; }
    }

    [SetUp]
    public void SetUp()
    {
        _nextEnumeratedSingleValue = 0;
    }

    [Test]
    public void NotNullAndEnumerated_guards_against_null()
    {
        var transformation = () => With.NotNullAndEnumerated(
            null! as IEnumerable<int>,
            _ => 1);

        transformation.Should().Throw<ArgumentException>().WithMessage("*elements*");
    }

    [Test]
    public void NotNullAndEnumerated_enumerates_elements_exactly_once()
    {
        var expectedSingleValue = _nextEnumeratedSingleValue;

        With.NotNullAndEnumerated(
                EnumeratesDifferentlyEachTime,
                xs => xs.Single())
            .Should().Be(expectedSingleValue);
    }

    [Test]
    public void NotNullOrEmptyAndEnumerated_guards_against_null()
    {
        var transformation = () => With.NotNullOrEmptyAndEnumerated(
            null! as IEnumerable<int>,
            _ => 1);

        transformation.Should().Throw<ArgumentException>().WithMessage("*elements*");
    }

    [Test]
    public void NotNullOrEmptyAndEnumerated_guards_against_empty()
    {
        var transformation = () => With.NotNullOrEmptyAndEnumerated(
            Enumerable.Empty<int>(),
            _ => 1);

        transformation.Should().Throw<ArgumentException>().WithMessage("*elements*");
    }

    [Test]
    public void NotNullOrEmptyAndEnumerated_enumerates_elements_exactly_once()
    {
        var expectedSingleValue = _nextEnumeratedSingleValue;

        With.NotNullOrEmptyAndEnumerated(
                EnumeratesDifferentlyEachTime,
                xs => xs.Single())
            .Should().Be(expectedSingleValue);
    }
}