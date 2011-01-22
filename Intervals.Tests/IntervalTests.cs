using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

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
        public void ConstructorForDataInterval_WithoutData_StoresDefaultIntoDataProperty()
        {
            var interval1 = new Interval<int, string>(0, 10);
            Assert.That(interval1.Data, Is.Null);

            var interval2 = new Interval<int, int>(0, 10);
            Assert.That(interval2.Data, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_NullEnd_ThrowsArgumentNullException()
        {
            var start = new NullableComparable(10);
            NullableComparable end = null;

            Assert.Throws<ArgumentNullException>(() => new Interval<NullableComparable>(start, end));
        }

        [Test]
        public void Constructor_NullStart_ThrowsArgumentNullException()
        {
            NullableComparable start = null;
            var end = new NullableComparable(10);

            Assert.Throws<ArgumentNullException>(() => new Interval<NullableComparable>(start, end));
        }

        [Test]
        public void Constructor_StartEqualsEnd_ThrowsArgumentOutOfRangeException()
        {
            var start = new NullableComparable(10);
            var end = new NullableComparable(10);

            Assert.Throws<ArgumentOutOfRangeException>(() => new Interval<NullableComparable>(start, end));
        }

        [Test]
        public void Constructor_StartGreaterThanEnd_ThrowsArgumentOutOfRangeException()
        {
            var start = new NullableComparable(11);
            var end = new NullableComparable(10);

            Assert.Throws<ArgumentOutOfRangeException>(() => new Interval<NullableComparable>(start, end));
        }

        [Test]
        public void Constructor_WithData_StoresDataIntoProperty()
        {
            var interval = new Interval<int, string>(0, 10, "test");

            Assert.That(interval.Data, Is.EqualTo("test"));
        }

        [Test]
        public void Create_NullEnd_ThrowsArgumentNullException()
        {
            var start = new NullableComparable(10);
            NullableComparable end = null;

            Assert.Throws<ArgumentNullException>(() => Interval.Create(start, end));
        }

        [Test]
        public void Create_NullStart_ThrowsArgumentNullException()
        {
            NullableComparable start = null;
            var end = new NullableComparable(10);

            Assert.Throws<ArgumentNullException>(() => Interval.Create(start, end));
        }

        [Test]
        public void Create_StartEqualsEnd_ThrowsArgumentOutOfRangeException()
        {
            var start = new NullableComparable(10);
            var end = new NullableComparable(10);

            Assert.Throws<ArgumentOutOfRangeException>(() => Interval.Create(start, end));
        }

        [Test]
        public void Create_StartGreaterThanEnd_ThrowsArgumentOutOfRangeException()
        {
            var start = new NullableComparable(11);
            var end = new NullableComparable(10);

            Assert.Throws<ArgumentOutOfRangeException>(() => Interval.Create(start, end));
        }

        [Test]
        public void Create_WithData_StoresDataIntoProperty()
        {
            Interval<int, string> interval = Interval.Create(0, 10, "test");

            Assert.That(interval.Data, Is.EqualTo("test"));
        }

        [Test]
        public void Equals_DifferentInstanceWithSameValues_ReturnsTrue()
        {
            var a = new Interval<int>(0, 10);
            var b = new Interval<int>(0, 10);

            bool result = a.Equals(b);
            Assert.That(result, Is.True);

            result = a.Equals((object)b);
            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_IntervalWithDataAgainstSameIntervalWithoutData_ComparesEqual()
        {
            Interval<int, string> a = Interval.Create(0, 10, "test");
            Interval<int> b = Interval.Create(0, 10);

            Assert.That(a.Equals(b), Is.True);
            Assert.That(a.Equals((object)b), Is.True);
            Assert.That(IntervalEqualityComparer<int>.Default.Equals(a, b), Is.True);
            Assert.That(IntervalComparer<int>.Default.Compare(a, b), Is.EqualTo(0));
        }

        [Test]
        public void Equals_Itself_ReturnsTrue()
        {
            var interval = new Interval<int>(0, 10);

            bool result = interval.Equals(interval);
            Assert.That(result, Is.True);

            result = interval.Equals((object)interval);
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
            IInterval<int> b = null;

            bool result = a.Equals(b);
            Assert.That(result, Is.False);

            result = a.Equals((object)b);
            Assert.That(result, Is.False);
        }

        [Test]
        public void GetHashCode_DifferentEnd_ReturnsDifferentValues()
        {
            var a = new Interval<int>(0, 10);
            var b = new Interval<int>(0, 9);

            Assert.That(a.GetHashCode(), Is.Not.EqualTo(b.GetHashCode()));
        }

        [Test]
        public void GetHashCode_DifferentInstancesWithSameValues_ReturnsSameValue()
        {
            var a = new Interval<int>(0, 10);
            var b = new Interval<int>(0, 10);

            Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
        }

        [Test]
        public void GetHashCode_DifferentStart_ReturnsDifferentValues()
        {
            var a = new Interval<int>(0, 10);
            var b = new Interval<int>(-1, 10);

            Assert.That(a.GetHashCode(), Is.Not.EqualTo(b.GetHashCode()));
        }

        [Test]
        public void ToString_ReturnsCorrectValue()
        {
            var interval = new Interval<int>(0, 10);

            Assert.That(interval.ToString(), Is.EqualTo("0..10"));
        }
    }
}