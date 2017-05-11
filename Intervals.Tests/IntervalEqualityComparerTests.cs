using System;
using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable SuspiciousTypeConversion.Global

namespace Intervals.Tests
{
    [TestFixture]
    public class IntervalEqualityComparerTests
    {
        [Test]
        public void Equals_DifferentInstanceWithSameValues_ReturnsTrue()
        {
            var a = new Interval<int>(0, 10);
            var b = new Interval<int>(0, 10);

            bool result = IntervalEqualityComparer<int>.Default.Equals(a, b);
            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_IntervalWithDataAgainstSameIntervalWithoutData_ComparesEqual()
        {
            TaggedInterval<int> a = TaggedInterval.Create(0, 10, "test");
            Interval<int> b = Interval.Create(0, 10);

            var result = IntervalEqualityComparer<int>.Default.Equals(a, b);
            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_Itself_ReturnsTrue()
        {
            var interval = new Interval<int>(0, 10);

            bool result = IntervalEqualityComparer<int>.Default.Equals(interval, interval);
            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_NotAnInterval_ReturnsFalse()
        {
            var interval = new Interval<int>(0, 10);

            bool result = interval.Equals("test");
            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_Null_ReturnsFalse()
        {
            var a = new Interval<int>(0, 10);

            bool result = IntervalEqualityComparer<int>.Default.Equals(a, null);
            Assert.That(result, Is.False);
        }

        [Test]
        public void GetHashCode_NullInterval_ReturnsZero()
        {
            var hashCode = IntervalEqualityComparer<int>.Default.GetHashCode(null);
            Assert.That(hashCode, Is.EqualTo(0));
        }

        [Test]
        public void GetHashCode_DifferentEnd_ReturnsDifferentValues()
        {
            var a = new Interval<int>(0, 10);
            var b = new Interval<int>(0, 9);

            var hashCodeA = IntervalEqualityComparer<int>.Default.GetHashCode(a);
            var hashCodeB = IntervalEqualityComparer<int>.Default.GetHashCode(b);
            Assert.That(hashCodeA, Is.Not.EqualTo(hashCodeB));
        }

        [Test]
        public void GetHashCode_DifferentInstancesWithSameValues_ReturnsSameValue()
        {
            var a = new Interval<int>(0, 10);
            var b = new Interval<int>(0, 10);

            var hashCodeA = IntervalEqualityComparer<int>.Default.GetHashCode(a);
            var hashCodeB = IntervalEqualityComparer<int>.Default.GetHashCode(b);
            Assert.That(hashCodeA, Is.EqualTo(hashCodeB));
        }

        [Test]
        public void GetHashCode_DifferentStart_ReturnsDifferentValues()
        {
            var a = new Interval<int>(0, 10);
            var b = new Interval<int>(-1, 10);

            var hashCodeA = IntervalEqualityComparer<int>.Default.GetHashCode(a);
            var hashCodeB = IntervalEqualityComparer<int>.Default.GetHashCode(b);
            Assert.That(hashCodeA, Is.Not.EqualTo(hashCodeB));
        }
    }
}
