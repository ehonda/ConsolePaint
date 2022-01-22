using System;
using System.Collections.Generic;
using ConsolePaint.Controls;
using FluentAssertions;
using NUnit.Framework;

namespace ConsolePaint.Tests.Controls;

[TestFixture]
public class CursorTests
{
    private static IEnumerable<Func<Cursor>> CreatingCursorWithNegativeXLimitTestCaseSource =>
        new Func<Cursor>[]
        {
            () => new(-1, 1),
            () => new(0, 0, -1, 1)
        };
    
    [TestCaseSource(nameof(CreatingCursorWithNegativeXLimitTestCaseSource))]
    public void Creating_cursor_with_negative_x_limit_throws(Func<Cursor> creatingCursor)
    {
        creatingCursor.Should().Throw<ArgumentException>().WithMessage("*xLimit*");
    }
    
    private static IEnumerable<Func<Cursor>> CreatingCursorWithNegativeYLimitTestCaseSource =>
        new Func<Cursor>[]
        {
            () => new(1, -1),
            () => new(0, 0, 1, -1)
        };
    
    [TestCaseSource(nameof(CreatingCursorWithNegativeYLimitTestCaseSource))]
    public void Creating_cursor_with_negative_y_limit_throws(Func<Cursor> creatingCursor)
    {
        creatingCursor.Should().Throw<ArgumentException>().WithMessage("*yLimit*");
    }
    
    [TestCase(-1)]
    [TestCase(2)]
    public void Creating_cursor_with_x_out_of_range_throws(int x)
    {
        Func<Cursor> creatingCursor = () => new(x, 0, 1, 1);
        creatingCursor.Should().Throw<ArgumentException>().WithMessage("*x*");
    }
    
    [TestCase(-1)]
    [TestCase(2)]
    public void Creating_cursor_with_y_out_of_range_throws(int y)
    {
        Func<Cursor> creatingCursor = () => new(0, y, 1, 1);
        creatingCursor.Should().Throw<ArgumentException>().WithMessage("*y*");
    }
}