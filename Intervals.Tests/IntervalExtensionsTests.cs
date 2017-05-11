using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable PossibleNullReferenceException
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable InvokeAsExtensionMethod

namespace Intervals.Tests
{
    [TestFixture]
    public class IntervalExtensionsTests
    {
        [Test]
        public void GetSpan_NullCases_ReturnsZero()
        {
            Assert.That(IntervalExtensions.GetSpan((IInterval<byte>)null), Is.EqualTo(0));
            Assert.That(IntervalExtensions.GetSpan((IInterval<short>)null), Is.EqualTo(0));
            Assert.That(IntervalExtensions.GetSpan((IInterval<int>)null), Is.EqualTo(0));
            Assert.That(IntervalExtensions.GetSpan((IInterval<long>)null), Is.EqualTo(0));
            Assert.That(IntervalExtensions.GetSpan((IInterval<double>)null), Is.EqualTo(0));
            Assert.That(IntervalExtensions.GetSpan((IInterval<float>)null), Is.EqualTo(0));
            Assert.That(IntervalExtensions.GetSpan((IInterval<decimal>)null), Is.EqualTo(0));
            Assert.That(IntervalExtensions.GetSpan((IInterval<DateTime>)null), Is.EqualTo(TimeSpan.Zero));
        }

        [TestCase(0, 10, 10)]
        [TestCase(-1, 10, 11)]
        [TestCase(1, 10, 9)]
        [TestCase(0, 11, 11)]
        [TestCase(0, 9, 9)]
        public void GetSpan_Int16Interval_ProducesCorrectValue(int start, int end, int expected)
        {
            Interval<short> interval = Interval.Create((short)start, (short)end);

            Assert.That(interval.GetSpan(), Is.EqualTo(expected));
        }

        [TestCase(0, 10, 10)]
        [TestCase(-1, 10, 11)]
        [TestCase(1, 10, 9)]
        [TestCase(0, 11, 11)]
        [TestCase(0, 9, 9)]
        public void GetSpan_DoubleInterval_ProducesCorrectValue(int start, int end, int expected)
        {
            Interval<double> interval = Interval.Create((double)start, (double)end);

            Assert.That(interval.GetSpan(), Is.EqualTo(expected));
        }

        [TestCase(0, 10, 10)]
        [TestCase(-1, 10, 11)]
        [TestCase(1, 10, 9)]
        [TestCase(0, 11, 11)]
        [TestCase(0, 9, 9)]
        public void GetSpan_SingleInterval_ProducesCorrectValue(int start, int end, int expected)
        {
            Interval<float> interval = Interval.Create((float)start, (float)end);

            Assert.That(interval.GetSpan(), Is.EqualTo(expected));
        }

        [TestCase(0, 10, 10)]
        [TestCase(-1, 10, 11)]
        [TestCase(1, 10, 9)]
        [TestCase(0, 11, 11)]
        [TestCase(0, 9, 9)]
        public void GetSpan_DecimalInterval_ProducesCorrectValue(int start, int end, int expected)
        {
            Interval<decimal> interval = Interval.Create((decimal)start, (decimal)end);

            Assert.That(interval.GetSpan(), Is.EqualTo(expected));
        }

        [Test]
        public void GetSpan_DateTimeInterval_ProducesCorrectValue()
        {
            var interval = Interval.Create(new DateTime(2016, 01, 02, 08, 00, 00), new DateTime(2016, 01, 03, 12, 30, 00));

            Assert.That(interval.GetSpan(), Is.EqualTo(TimeSpan.FromHours(28.5)));
        }


        [TestCase(0, 10, 10)]
        [TestCase(-1, 10, 11)]
        [TestCase(1, 10, 9)]
        [TestCase(0, 11, 11)]
        [TestCase(0, 9, 9)]
        public void GetSpan_Int32Interval_ProducesCorrectValue(int start, int end, int expected)
        {
            Interval<int> interval = Interval.Create(start, end);

            Assert.That(interval.GetSpan(), Is.EqualTo(expected));
        }

        [TestCase(0, 10, 10)]
        [TestCase(-1, 10, 11)]
        [TestCase(1, 10, 9)]
        [TestCase(0, 11, 11)]
        [TestCase(0, 9, 9)]
        public void GetSpan_Int64Interval_ProducesCorrectValue(int start, int end, int expected)
        {
            Interval<long> interval = Interval.Create((long)start, (long)end);

            Assert.That(interval.GetSpan(), Is.EqualTo(expected));
        }

        [TestCase(0, 10, 10)]
        [TestCase(1, 10, 9)]
        [TestCase(0, 11, 11)]
        [TestCase(0, 9, 9)]
        public void GetSpan_ByteInterval_ProducesCorrectValue(int start, int end, int expected)
        {
            Interval<byte> interval = Interval.Create((byte)start, (byte)end);

            Assert.That(interval.GetSpan(), Is.EqualTo(expected));
        }

        [Test]
        public void TryGetUnion_NullInterval_ReturnsNull()
        {
            IInterval<int> a = null;
            IInterval<int> b = Interval.Create(15, 25);

            var union = a.TryGetUnion(b);

            Assert.That(union, Is.Null);
        }

        [Test]
        public void TryGetUnion_NullOther_ReturnsNull()
        {
            IInterval<int> a = Interval.Create(10, 20);
            IInterval<int> b = null;

            var union = a.TryGetUnion(b);

            Assert.That(union, Is.Null);
        }

        [Test]
        public void TryGetUnion_OverlappingIntervals_ReturnsUnion()
        {
            IInterval<int> a = Interval.Create(10, 20);
            IInterval<int> b = Interval.Create(15, 25);

            var union = a.TryGetUnion(b);
            var expected = Interval.Create(10, 25);

            Assert.That(union, Is.EqualTo(expected).Using(IntervalEqualityComparer<int>.Default));
        }

        [Test]
        public void TryGetUnion_AdjacentIntervals_ReturnsUnion()
        {
            IInterval<int> a = Interval.Create(10, 20);
            IInterval<int> b = Interval.Create(20, 25);

            var union = a.TryGetUnion(b);
            var expected = Interval.Create(10, 25);

            Assert.That(union, Is.EqualTo(expected).Using(IntervalEqualityComparer<int>.Default));
        }

        [Test]
        public void TryGetUnion_IntervalsThatDoNotOverlapNorAreAdjacent_ReturnsNull()
        {
            IInterval<int> a = Interval.Create(10, 20);
            IInterval<int> b = Interval.Create(25, 30);

            var union = a.TryGetUnion(b);

            Assert.That(union, Is.Null);
        }

        [Test]
        public void IsAdjacentTo_NullInterval_ReturnsFalse()
        {
            IInterval<int> a = null;
            IInterval<int> b = Interval.Create(15, 25);

            var result = a.IsAdjacentTo(b);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsAdjacentTo_NullOther_ReturnsFalse()
        {
            IInterval<int> a = Interval.Create(10, 20);
            IInterval<int> b = null;

            var result = a.IsAdjacentTo(b);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsAdjacentTo_OverlappingIntervals_ReturnsFalse()
        {
            IInterval<int> a = Interval.Create(10, 20);
            IInterval<int> b = Interval.Create(15, 25);

            var result = a.IsAdjacentTo(b);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsAdjacentTo_AIsAdjacentToAndJustBeforeB_ReturnsTrue()
        {
            IInterval<int> a = Interval.Create(10, 20);
            IInterval<int> b = Interval.Create(20, 25);

            var result = a.IsAdjacentTo(b);

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsAdjacentTo_AIsAdjacentToAndJustAfterB_ReturnsTrue()
        {
            IInterval<int> a = Interval.Create(20, 25);
            IInterval<int> b = Interval.Create(10, 20);

            var result = a.IsAdjacentTo(b);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Union_NullInterval_ThrowsInvalidOperationException()
        {
            IInterval<int> a = null;
            IInterval<int> b = Interval.Create(15, 25);

            Assert.Throws<InvalidOperationException>(() => a.GetUnion(b));
        }

        [Test]
        public void Union_NullOther_ThrowsInvalidOperationException()
        {
            IInterval<int> a = Interval.Create(10, 20);
            IInterval<int> b = null;

            Assert.Throws<InvalidOperationException>(() => a.GetUnion(b));
        }

        [Test]
        public void Union_OverlappingIntervals_ReturnsUnion()
        {
            IInterval<int> a = Interval.Create(10, 20);
            IInterval<int> b = Interval.Create(15, 25);

            var union = a.GetUnion(b);
            var expected = Interval.Create(10, 25);

            Assert.That(union, Is.EqualTo(expected).Using(IntervalEqualityComparer<int>.Default));
        }

        [Test]
        public void Union_AdjacentIntervals_ReturnsUnion()
        {
            IInterval<int> a = Interval.Create(10, 20);
            IInterval<int> b = Interval.Create(20, 25);

            var union = a.GetUnion(b);
            var expected = Interval.Create(10, 25);

            Assert.That(union, Is.EqualTo(expected).Using(IntervalEqualityComparer<int>.Default));
        }

        [Test]
        public void Union_IntervalsThatDoNotOverlapNorAreAdjacent_ThrowsInvalidOperationException()
        {
            IInterval<int> a = Interval.Create(10, 20);
            IInterval<int> b = Interval.Create(25, 30);

            Assert.Throws<InvalidOperationException>(() => a.GetUnion(b));
        }

        [Test]
        public void Merge_NullIntervals_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => IntervalExtensions.Merge<int>(null));
        }

        [Test]
        public void IntervalTo_StartAfterEnd_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => IntervalExtensions.IntervalTo(10, 5));
        }

        [Test]
        public void IntervalTo_StartLessThanEnd_ReturnsInterval()
        {
            var interval = IntervalExtensions.IntervalTo(5, 10);

            Assert.That(interval, Is.EqualTo(new Interval<int>(5, 10)).Using(IntervalEqualityComparer<int>.Default));
        }

        [Test]
        public void IntervalToWithTag_StartAfterEnd_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => IntervalExtensions.IntervalTo(10, 5, "tag"));
        }

        [Test]
        public void IntervalToWithTag_StartLessThanEnd_ReturnsInterval()
        {
            var interval = IntervalExtensions.IntervalTo(5, 10, "tag");

            Assert.That(interval, Is.EqualTo(new Interval<int>(5, 10)).Using(IntervalEqualityComparer<int>.Default));
        }

        [Test]
        public void Equals_NullInterval_ReturnsFalse()
        {
            Interval<int> i1 = null;
            Interval<int> i2 = Interval.Create(5, 10);

            Assert.That(IntervalExtensions.Equals(i1, i2), Is.False);
        }

        [Test]
        public void Equals_NullOther_ReturnsFalse()
        {
            Interval<int> i1 = Interval.Create(5, 10);
            Interval<int> i2 = null;

            Assert.That(IntervalExtensions.Equals(i1, i2), Is.False);
        }

        [Test]
        public void Equals_NullIntervalAndOther_ReturnsTrue()
        {
            Interval<int> i1 = null;
            Interval<int> i2 = null;

            Assert.That(IntervalExtensions.Equals(i1, i2), Is.True);
        }

        [Test]
        public void Equals_DifferentIntervalEnds_ReturnsFalse()
        {
            Interval<int> i1 = Interval.Create(5, 10);
            Interval<int> i2 = Interval.Create(5, 15);

            Assert.That(IntervalExtensions.Equals(i1, i2), Is.False);
        }

        [Test]
        public void Equals_DifferentIntervalStarts_ReturnsFalse()
        {
            Interval<int> i1 = Interval.Create(2, 10);
            Interval<int> i2 = Interval.Create(5, 10);

            Assert.That(IntervalExtensions.Equals(i1, i2), Is.False);
        }

        [Test]
        public void Equals_SameIntervalStartAndEnd()
        {
            Interval<int> i1 = Interval.Create(5, 10);
            Interval<int> i2 = Interval.Create(5, 10);

            Assert.That(IntervalExtensions.Equals(i1, i2), Is.True);
        }

        [Test]
        [TestCase(5, 10, 5, true)]
        [TestCase(5, 10, 9, true)]
        [TestCase(5, 10, 10, false)]
        [TestCase(5, 10, 4, false)]
        public void Contains_WithTestCases_ProducesExpectedResults(int start, int end, int value, bool expected)
        {
            var interval = Interval.Create(start, end);

            var output = IntervalExtensions.Contains(interval, value);

            Assert.That(output, Is.EqualTo(expected));
        }

        [Test]
        public void Contains_NullInterval_ReturnsFalse()
        {
            var output = IntervalExtensions.Contains(null, 0);
            Assert.That(output, Is.False);
        }

        [Test]
        public void IsOverlapping_SameInterval_ReturnsTrue()
        {
            var interval = 10.IntervalTo(20);

            var output = IntervalExtensions.IsOverlapping(interval, interval);

            Assert.That(output, Is.True);
        }

        [Test]
        public void IsOverlapping_NullInterval_ReturnsFalse()
        {
            Interval<int> interval = null;
            Interval<int> other = 10.IntervalTo(20);

            var output = IntervalExtensions.IsOverlapping(interval, other);

            Assert.That(output, Is.False);
        }

        [Test]
        public void IsOverlapping_NullOther_ReturnsFalse()
        {
            Interval<int> interval = 10.IntervalTo(20);
            Interval<int> other = null;

            var output = IntervalExtensions.IsOverlapping(interval, other);

            Assert.That(output, Is.False);
        }

        [Test]
        public void IsOverlapping_NullIntervalAndOther_ReturnsFalse()
        {
            Interval<int> interval = null;
            Interval<int> other = null;

            var output = IntervalExtensions.IsOverlapping(interval, other);

            Assert.That(output, Is.False);
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
            Interval<int> interval = Interval.Create(start1, end1);
            Interval<int> other = Interval.Create(start2, end2);

            var output = IntervalExtensions.IsOverlapping(interval, other);

            Assert.That(output, Is.EqualTo(expected));
        }

        [Test]
        public void TryGetOverlappingIntervals_NullInterval_ReturnsNull()
        {
            Interval<int> interval = null;
            Interval<int> other = Interval.Create(5, 15);

            var output = IntervalExtensions.TryGetOverlappingInterval(interval, other);

            Assert.That(output, Is.Null);
        }

        [Test]
        public void TryGetOverlappingIntervals_NullOther_ReturnsNull()
        {
            Interval<int> interval = Interval.Create(5, 15);
            Interval<int> other = null;

            var output = IntervalExtensions.TryGetOverlappingInterval(interval, other);

            Assert.That(output, Is.Null);
        }

        [Test]
        public void TryGetOverlappingIntervals_NullIntervalAndOther_ReturnsNull()
        {
            Interval<int> interval = null;
            Interval<int> other = null;

            var output = IntervalExtensions.TryGetOverlappingInterval(interval, other);

            Assert.That(output, Is.Null);
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
            Interval<int> interval = Interval.Create(start1, end1);
            Interval<int> other = Interval.Create(start2, end2);

            Interval<int> expected = (expectedStart > expectedEnd) ? null : Interval.Create(expectedStart, expectedEnd);

            var output = IntervalExtensions.TryGetOverlappingInterval(interval, other);

            Assert.That(output, Is.EqualTo(expected).Using(IntervalEqualityComparer<int>.Default));
        }

        [Test]
        public void IsValid_NullInterval_ReturnsFalse()
        {
            Assert.That(IntervalExtensions.IsValid<int>(null), Is.False);
        }

        [TestCase(0, 10, true)]
        [TestCase(0, 0, true)]
        [TestCase(0, -1, false)]
        [TestCase(5, 4, false)]
        [TestCase(5, 5, true)]
        public void IsValid_WithTestCases_ProducesCorrectResults(int start, int end, bool expected)
        {
            var interval = new StupidInterval(start, end);

            var output = IntervalExtensions.IsValid(interval);

            Assert.That(output, Is.EqualTo(expected));
        }

        public class StupidInterval : IInterval<int>
        {
            public StupidInterval(int start, int end)
            {
                Start = start;
                End = end;
            }

            public int Start
            {
                get;
            }

            public int End
            {
                get;
            }
        }
    }
}