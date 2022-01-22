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

    [Test]
    public void Cursor_moves_upwards_cyclically()
    {
        var cursor = new Cursor(0, 3);
        cursor.Y.Should().Be(0);
        
        cursor.Move(Direction.Up);
        cursor.Y.Should().Be(1);
        
        cursor.Move(Direction.Up);
        cursor.Y.Should().Be(2);
        
        cursor.Move(Direction.Up);
        cursor.Y.Should().Be(0);
    }
    
    [Test]
    public void Cursor_moves_downwards_cyclically()
    {
        var cursor = new Cursor(0, 3);
        cursor.Y.Should().Be(0);
        
        cursor.Move(Direction.Down);
        cursor.Y.Should().Be(2);
        
        cursor.Move(Direction.Down);
        cursor.Y.Should().Be(1);
        
        cursor.Move(Direction.Down);
        cursor.Y.Should().Be(0);
    }
    
    [Test]
    public void Cursor_moves_to_the_right_cyclically()
    {
        var cursor = new Cursor(3, 0);
        cursor.X.Should().Be(0);
        
        cursor.Move(Direction.Right);
        cursor.X.Should().Be(1);
        
        cursor.Move(Direction.Right);
        cursor.X.Should().Be(2);
        
        cursor.Move(Direction.Right);
        cursor.X.Should().Be(0);
    }
    
    [Test]
    public void Cursor_moves_to_the_left_cyclically()
    {
        var cursor = new Cursor(3, 0);
        cursor.X.Should().Be(0);
        
        cursor.Move(Direction.Left);
        cursor.X.Should().Be(2);
        
        cursor.Move(Direction.Left);
        cursor.X.Should().Be(1);
        
        cursor.Move(Direction.Left);
        cursor.X.Should().Be(0);
    }
}