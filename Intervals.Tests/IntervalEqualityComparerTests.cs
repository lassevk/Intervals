using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace Intervals.Tests
{
    [TestFixture]
    public class IntervalEqualityComparerTests
    {
        [Test]
        public void Equals_DifferentEnd_ReturnsFalse()
        {
            IInterval<int> a = new Interval<int>(0, 10);
            IInterval<int> b = new Interval<int>(0, 11);

            Assert.That(IntervalEqualityComparer<int>.Default.Equals(a, b), Is.False);
        }

        [Test]
        public void Equals_DifferentInstancesSameValues_ReturnsTrue()
        {
            IInterval<int> a = new Interval<int>(0, 10);
            IInterval<int> b = new Interval<int>(0, 10);

            Assert.That(IntervalEqualityComparer<int>.Default.Equals(a, b), Is.True);
        }

        [Test]
        public void Equals_DifferentStart_ReturnsFalse()
        {
            IInterval<int> a = new Interval<int>(0, 10);
            IInterval<int> b = new Interval<int>(1, 10);

            Assert.That(IntervalEqualityComparer<int>.Default.Equals(a, b), Is.False);
        }

        [Test]
        public void Equals_NullX_ReturnsFalse()
        {
            IInterval<int> a = null;
            IInterval<int> b = new Interval<int>(0, 10);

            Assert.That(IntervalEqualityComparer<int>.Default.Equals(a, b), Is.False);
        }

        [Test]
        public void Equals_NullXandY_ReturnsTrue()
        {
            IInterval<int> a = null;
            IInterval<int> b = null;

            Assert.That(IntervalEqualityComparer<int>.Default.Equals(a, b), Is.True);
        }

        [Test]
        public void Equals_NullY_ReturnsFalse()
        {
            IInterval<int> a = new Interval<int>(0, 10);
            IInterval<int> b = null;

            Assert.That(IntervalEqualityComparer<int>.Default.Equals(a, b), Is.False);
        }

        [Test]
        public void Equals_SameInstance_ReturnsTrue()
        {
            IInterval<int> a = new Interval<int>(0, 10);
            IInterval<int> b = a;

            Assert.That(IntervalEqualityComparer<int>.Default.Equals(a, b), Is.True);
        }

        [Test]
        public void GetHashCode_NullArgument_ThrowsArgumentNullException()
        {
            IInterval<int> a = null;

            Assert.Throws<ArgumentNullException>((() => IntervalEqualityComparer<int>.Default.GetHashCode(a)));
        }
    }
}