using System;
using NUnit.Framework;

// ReSharper disable EqualExpressionComparison
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable SuspiciousTypeConversion.Global
// ReSharper disable PossibleNullReferenceException

namespace Intervals.Tests
{
    [TestFixture]
    public class IntervalTests
    {
        [TestCase(10)]
        [TestCase(-10)]
        [TestCase(0)]
        public void Start_ContainsValueGivenToConstructor(int input)
        {
            var interval = new Interval<int>(input, input + 1);

            Assert.That(interval.Start, Is.EqualTo(input));
        }

        [TestCase(10)]
        [TestCase(-10)]
        [TestCase(0)]
        public void End_ContainsValueGivenToConstructor(int input)
        {
            var interval = new Interval<int>(input, input + 1);

            Assert.That(interval.End, Is.EqualTo(input + 1));
        }

        [TestCase(0, 10, 0, 10, 0)]
        [TestCase(0, 10, 0, 9, +1)] // a comes after b
        [TestCase(0, 10, 0, 11, -1)] // a comes before b
        [TestCase(0, 10, -1, 10, +1)] // a comes after b
        [TestCase(0, 10, 1, 10, -1)] // a comes before b
        public void Compare_TestCases_ProducesCorrectResults(int interval1Start, int interval1End, int interval2Start, int interval2End, int expected)
        {
            var a = new Interval<int>(interval1Start, interval1End);
            var b = new Interval<int>(interval2Start, interval2End);

            int result = a.CompareTo(b);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Constructor_StartEqualsEnd_ReturnsEmptyInterval()
        {
            var start = new NullableComparable(10);
            var end = new NullableComparable(10);

            var interval = new Interval<NullableComparable>(start, end);

            Assert.That(interval.IsEmpty(), Is.True);
        }

        [Test]
        public void Constructor_StartGreaterThanEnd_ThrowsArgumentOutOfRangeException()
        {
            var start = new NullableComparable(11);
            var end = new NullableComparable(10);

            Assert.Throws<ArgumentOutOfRangeException>(() => new Interval<NullableComparable>(start, end));
        }

        [Test]
        public void Create_StartEqualsEnd_ReturnsEmptyInterval()
        {
            var start = new NullableComparable(10);
            var end = new NullableComparable(10);

            var interval = Interval.Create(start, end);

            Assert.That(interval.IsEmpty(), Is.True);
        }

        [Test]
        public void Create_StartGreaterThanEnd_ThrowsArgumentOutOfRangeException()
        {
            var start = new NullableComparable(11);
            var end = new NullableComparable(10);

            Assert.Throws<ArgumentOutOfRangeException>(() => Interval.Create(start, end));
        }


        [Test]
        public void ToString_ReturnsCorrectValue()
        {
            var interval = new Interval<int>(0, 10);

            Assert.That(interval.ToString(), Is.EqualTo("[0, 10)"));
        }

        [Test]
        public void TryGetOverlappingInterval_NullInterval2_ReturnsNull()
        {
            var interval1 = Interval.Create(0, 10);
            Interval<int> interval2 = null;

            var overlappingInterval = interval1.TryGetOverlappingInterval(interval2);

            Assert.That(overlappingInterval, Is.Null);
        }

        [Test]
        public void TryGetOverlappingInterval_NoOverlap_ReturnsNull()
        {
            var interval1 = Interval.Create(0, 10);
            var interval2 = Interval.Create(15, 20);

            var overlappingInterval = interval1.TryGetOverlappingInterval(interval2);

            Assert.That(overlappingInterval, Is.Null);
        }

        [Test]
        public void TryGetOverlappingInterval_TouchingIntervalsThatDoesNotOverlap_ReturnsNull()
        {
            var interval1 = Interval.Create(0, 10);
            var interval2 = Interval.Create(10, 20);

            var overlappingInterval = interval1.TryGetOverlappingInterval(interval2);

            Assert.That(overlappingInterval, Is.Null);
        }

        [Test]
        public void TryGetOverlappingInterval_IntervalsThatOverlapOnOneValue_ReturnsIntervalContainingThatValue()
        {
            var interval1 = Interval.Create(0, 10);
            var interval2 = Interval.Create(9, 20);

            var overlappingInterval = interval1.TryGetOverlappingInterval(interval2);
            var expected = Interval.Create(9, 10);

            Assert.That(overlappingInterval, Is.EqualTo(expected).Using(IntervalEqualityComparer<int>.Default));
        }

        [Test]
        public void GetOverlappingInterval_NullInterval2_ThrowsInvalidOperationException()
        {
            var interval1 = Interval.Create(0, 10);
            Interval<int> interval2 = null;

            Assert.Throws<InvalidOperationException>(() => interval1.GetOverlappingInterval(interval2));
        }

        [Test]
        public void GetOverlappingInterval_NoOverlap_ThrowsInvalidOperationException()
        {
            var interval1 = Interval.Create(0, 10);
            var interval2 = Interval.Create(15, 20);

            Assert.Throws<InvalidOperationException>(() => interval1.GetOverlappingInterval(interval2));
        }

        [Test]
        public void GetOverlappingInterval_TouchingIntervalsThatDoesNotOverlap_ThrowsInvalidOperationException()
        {
            var interval1 = Interval.Create(0, 10);
            var interval2 = Interval.Create(10, 20);

            Assert.Throws<InvalidOperationException>(() => interval1.GetOverlappingInterval(interval2));
        }

        [Test]
        public void GetOverlappingInterval_IntervalsThatOverlapOnOneValue_ReturnsIntervalContainingThatValue()
        {
            var interval1 = Interval.Create(0, 10);
            var interval2 = Interval.Create(9, 20);

            var overlappingInterval = interval1.GetOverlappingInterval(interval2);
            var expected = Interval.Create(9, 10);

            Assert.That(overlappingInterval, Is.EqualTo(expected).Using(IntervalEqualityComparer<int>.Default));
        }

        [Test]
        public void IntervalTo_SameValue_ReturnsEmptyInterval()
        {
            var interval = 10.IntervalTo(10);

            Assert.That(interval.IsEmpty(), Is.True);
        }

        [Test]
        public void Equals_NullOther_ReturnsFalse()
        {
            var interval = 10.IntervalTo(10);

            var output = interval.Equals(null);

            Assert.That(output, Is.False);
        }

        [TestCase(0, 10)]
        [TestCase(0, 0)]
        [TestCase(5, 10)]
        [TestCase(0, 15)]
        [TestCase(5, 15)]
        public void GetHashCode_WithTestCases_ReturnsSameResultsAsIntervalEqualityComparerGetHashCode(int start, int end)
        {
            var interval = Interval.Create(start, end);

            var hashCode1 = interval.GetHashCode();
            var hashCode2 = IntervalEqualityComparer<int>.Default.GetHashCode(interval);

            Assert.That(hashCode1, Is.EqualTo(hashCode2));
        }

    }
}