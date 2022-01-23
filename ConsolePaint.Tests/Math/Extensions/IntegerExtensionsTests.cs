using ConsolePaint.Math.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace ConsolePaint.Tests.Math.Extensions;

[TestFixture]
public class IntegerExtensionsTests
{
    [TestCase(0, 0, 3, 0)]
    [TestCase(0, 1, 3, 1)]
    [TestCase(0, 2, 3, 2)]
    [TestCase(0, 3, 3, 0)]
    public void AddModulo_works_for_positive_numbers(int a, int b, int n, int expectedResult)
    {
        a.AddModulo(b, n).Should().Be(expectedResult);
    }
    
    [TestCase(0, -1, 3, 2)]
    [TestCase(0, -4, 3, 2)]
    [TestCase(0, -5, 3, 1)]
    public void AddModulo_works_for_negative_numbers_and_returns_positive_number(
        int a, int b, int n, int expectedResult)
    {
        a.AddModulo(b, n).Should().Be(expectedResult);
    }
}