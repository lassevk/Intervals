using System;
using NUnit.Framework;

namespace Intervals.Tests;

public class IntervalExtensionsTests
{
    [Test]
    public void TryGetUnion_OverlappingIntervals_ReturnsUnion()
    {
        var a = Interval.Create(10, 20);
        var b = Interval.Create(15, 25);

        Interval<int, bool>? union = a.TryGetUnion(b, false);
        var expected = Interval.Create(10, 25);

        Assert.That(union, Is.EqualTo(expected));
    }

    [Test]
    public void TryGetUnion_AdjacentIntervals_ReturnsUnion()
    {
        var a = Interval.Create(10, 20);
        var b = Interval.Create(20, 25);

        Interval<int, bool>? union = a.TryGetUnion(b, false);
        var expected = Interval.Create(10, 25);

        Assert.That(union, Is.EqualTo(expected));
    }

    [Test]
    public void TryGetUnion_IntervalsThatDoNotOverlapNorAreAdjacent_ReturnsNull()
    {
        var a = Interval.Create(10, 20);
        var b = Interval.Create(25, 30);

        Interval<int, bool>? union = a.TryGetUnion(b, false);

        Assert.That(union, Is.Null);
    }

    [Test]
    public void IsAdjacentTo_OverlappingIntervals_ReturnsFalse()
    {
        var a = Interval.Create(10, 20);
        var b = Interval.Create(15, 25);

        bool result = a.IsAdjacentTo(b);

        Assert.That(result, Is.False);
    }

    [Test]
    public void IsAdjacentTo_AIsAdjacentToAndJustBeforeB_ReturnsTrue()
    {
        var a = Interval.Create(10, 20);
        var b = Interval.Create(20, 25);

        bool result = a.IsAdjacentTo(b);

        Assert.That(result, Is.True);
    }

    [Test]
    public void IsAdjacentTo_AIsAdjacentToAndJustAfterB_ReturnsTrue()
    {
        var a = Interval.Create(20, 25);
        var b = Interval.Create(10, 20);

        bool result = a.IsAdjacentTo(b);

        Assert.That(result, Is.True);
    }

    [Test]
    public void Union_OverlappingIntervals_ReturnsUnion()
    {
        var a = Interval.Create(10, 20);
        var b = Interval.Create(15, 25);

        Interval<int, bool> union = a.GetUnion(b, false);
        var expected = Interval.Create(10, 25);

        Assert.That(union, Is.EqualTo(expected));
    }

    [Test]
    public void Union_AdjacentIntervals_ReturnsUnion()
    {
        var a = Interval.Create(10, 20);
        var b = Interval.Create(20, 25);

        Interval<int, bool> union = a.GetUnion(b, false);
        var expected = Interval.Create(10, 25);

        Assert.That(union, Is.EqualTo(expected));
    }

    [Test]
    public void Union_IntervalsThatDoNotOverlapNorAreAdjacent_ThrowsInvalidOperationException()
    {
        var a = Interval.Create(10, 20);
        var b = Interval.Create(25, 30);

        Assert.Throws<InvalidOperationException>(() => a.GetUnion(b, false));
    }

    [Test]
    public void Merge_NullIntervals_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => IntervalExtensions.Merge<int, bool>(null!));
    }

    [Test]
    public void IntervalTo_StartAfterEnd_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => 10.IntervalTo(5));
    }

    [Test]
    public void IntervalTo_StartLessThanEnd_ReturnsInterval()
    {
        Interval<int, bool> interval = 5.IntervalTo(10);

        Assert.That(interval, Is.EqualTo(new Interval<int, bool>(5, 10)));
    }

    [Test]
    public void IntervalToWithTag_StartAfterEnd_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => 10.IntervalTo(5, "tag"));
    }

    [Test]
    public void IntervalToWithTag_StartLessThanEnd_ReturnsInterval()
    {
        Interval<int, string> interval = 5.IntervalTo(10, "tag");

        Assert.That(interval, Is.EqualTo(new Interval<int, string>(5, 10, "tag")));
    }

    [Test]
    public void Equals_DifferentIntervalEnds_ReturnsFalse()
    {
        var i1 = Interval.Create(5, 10);
        var i2 = Interval.Create(5, 15);

        Assert.That(i1.Equals(i2), Is.False);
    }

    [Test]
    public void Equals_DifferentIntervalStarts_ReturnsFalse()
    {
        var i1 = Interval.Create(2, 10);
        var i2 = Interval.Create(5, 10);

        Assert.That(i1.Equals(i2), Is.False);
    }

    [Test]
    public void Equals_SameIntervalStartAndEnd()
    {
        var i1 = Interval.Create(5, 10);
        var i2 = Interval.Create(5, 10);

        Assert.That(i1.Equals(i2), Is.True);
    }

    [Test]
    [TestCase(5, 10, 5, true)]
    [TestCase(5, 10, 9, true)]
    [TestCase(5, 10, 10, false)]
    [TestCase(5, 10, 4, false)]
    public void Contains_WithTestCases_ProducesExpectedResults(int start, int end, int value, bool expected)
    {
        var interval = Interval.Create(start, end);

        bool output = interval.Contains(value);

        Assert.That(output, Is.EqualTo(expected));
    }

    [Test]
    public void IsOverlapping_SameInterval_ReturnsTrue()
    {
        Interval<int, bool> interval = 10.IntervalTo(20);

        bool output = interval.IsOverlapping(interval);

        Assert.That(output, Is.True);
    }

    [TestCase(0, 10, 10, 20, false)]
    [TestCase(0, 10, 15, 20, false)]
    [TestCase(0, 10, 5, 20, true)]
    [TestCase(0, 10, 9, 20, true)]
    [TestCase(0, 10, -10, 0, false)]
    [TestCase(0, 10, -10, 1, true)]
    [TestCase(0, 10, 0, 10, true)]
    [TestCase(0, 10, -10, 20, true)]
    [TestCase(0, 10, -10, 5, true)]
    public void IsOverlapping_WithTestCases_ProducesCorrectResults(int start1, int end1, int start2, int end2, bool expected)
    {
        var interval = Interval.Create(start1, end1);
        var other = Interval.Create(start2, end2);

        bool output = interval.IsOverlapping(other);

        Assert.That(output, Is.EqualTo(expected));
    }

    [TestCase(0, 10, 10, 20, 0, -1)]
    [TestCase(0, 10, 15, 20, 0, -1)]
    [TestCase(0, 10, 9, 20, 9, 10)]
    [TestCase(0, 10, 0, 10, 0, 10)]
    [TestCase(0, 10, 0, 20, 0, 10)]
    [TestCase(0, 10, -10, 20, 0, 10)]
    [TestCase(0, 10, -10, 5, 0, 5)]
    public void TryGetOverlappingIntervals_WithTestCases_ProducesCorrectResults(int start1, int end1, int start2, int end2, int expectedStart, int expectedEnd)
    {
        var interval = Interval.Create(start1, end1);
        var other = Interval.Create(start2, end2);

        Interval<int, bool>? expected = expectedStart > expectedEnd ? null : Interval.Create(expectedStart, expectedEnd);

        Interval<int, bool>? output = interval.TryGetOverlappingInterval(other, false);

        Assert.That(output, Is.EqualTo(expected));
    }
}