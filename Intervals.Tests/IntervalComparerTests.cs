using NUnit.Framework;

namespace Intervals.Tests;

public class IntervalComparerTests
{
    [TestCase(0, 10, 10, 20, -1)]
    [TestCase(10, 20, 0, 10, +1)]
    [TestCase(0, 10, 0, 10, 0)]
    [TestCase(0, 10, 0, 11, -1)]
    [TestCase(0, 10, 0, 9, +1)]
    [TestCase(1, 10, 0, 10, +1)]
    [TestCase(-1, 10, 0, 10, -1)]
    public void Compare_WithTestCases_ProducesCorrectResults(int start1, int end1, int start2, int end2, int expected)
    {
        var x = Interval.Create(start1, end1);
        var y = Interval.Create(start2, end2);

        int output = x.CompareTo(y);

        Assert.That(output, Is.EqualTo(expected));
    }
}