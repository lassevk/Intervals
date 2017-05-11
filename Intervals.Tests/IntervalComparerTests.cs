using System;
using NUnit.Framework;

// ReSharper disable ExpressionIsAlwaysNull

namespace Intervals.Tests
{
    [TestFixture]
    public class IntervalComparerTests
    {
        [Test]
        public void Compare_NullX_ReturnsMinusOne()
        {
            Interval<int> x = null;
            Interval<int> y = Interval.Create(5, 15);

            var output = IntervalComparer<int>.Default.Compare(x, y);
            int expected = -1;

            Assert.That(output, Is.EqualTo(expected));
        }

        [Test]
        public void Compare_NullY_ReturnsPlusOne()
        {
            Interval<int> x = Interval.Create(5, 15); 
            Interval<int> y = null;

            var output = IntervalComparer<int>.Default.Compare(x, y);
            int expected = +1;

            Assert.That(output, Is.EqualTo(expected));
        }

        [Test]
        public void Compare_NullXAndY_ReturnsZero()
        {
            Interval<int> x = null;
            Interval<int> y = null;

            var output = IntervalComparer<int>.Default.Compare(x, y);
            int expected = 0;

            Assert.That(output, Is.EqualTo(expected));
        }

        [TestCase(0, 10, 10, 20, -1)]
        [TestCase(10, 20, 0, 10, +1)]
        [TestCase(0, 10, 0, 10, 0)]
        [TestCase(0, 10, 0, 11, -1)]
        [TestCase(0, 10, 0, 9, +1)]
        [TestCase(1, 10, 0, 10, +1)]
        [TestCase(-1, 10, 0, 10, -1)]
        public void Compare_WithTestCases_ProducesCorrectResults(int start1, int end1, int start2, int end2, int expected)
        {
            Interval<int> x = Interval.Create(start1, end1);
            Interval<int> y = Interval.Create(start2, end2);

            var output = IntervalComparer<int>.Default.Compare(x, y);

            Assert.That(output, Is.EqualTo(expected));
        }
    }
}
