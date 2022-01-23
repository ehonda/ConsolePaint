using ConsolePaint.Math.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace ConsolePaint.Tests.Math.Extensions;

[TestFixture]
public class ComparableExtensionsTests
{
    [TestCase(1, 0, true)]
    [TestCase(0, 1, false)]
    [TestCase(1, 1, false)]
    public void IsGreaterThan_works(int a, int b, bool expectedResult)
    {
        a.IsGreaterThan(b).Should().Be(expectedResult);
    }

    [TestCase(1, 0, false)]
    [TestCase(1, 1, false)]
    [TestCase(0, 1, true)]
    public void IsLowerThan_works(int a, int b, bool expectedResult)
    {
        a.IsLowerThan(b).Should().Be(expectedResult);
    }

    [TestCase(1, 0, true)]
    [TestCase(0, 1, false)]
    [TestCase(1, 1, true)]
    public void IsGreaterThanOrEqualTo_works(int a, int b, bool expectedResult)
    {
        a.IsGreaterThanOrEqualTo(b).Should().Be(expectedResult);
    }

    [TestCase(1, 0, false)]
    [TestCase(1, 1, true)]
    [TestCase(0, 1, true)]
    public void IsLowerThanOrEqualTo_works(int a, int b, bool expectedResult)
    {
        a.IsLowerThanOrEqualTo(b).Should().Be(expectedResult);
    }

    [TestCase(1, 0, false)]
    [TestCase(1, 1, true)]
    public void IsEqualTo_works(int a, int b, bool expectedResult)
    {
        a.IsEqualTo(b).Should().Be(expectedResult);
    }
}