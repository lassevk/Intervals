using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace Intervals.Tests
{
    [TestFixture]
    public class IntervalComparerTests
    {
        [Test]
        public void Compare_NullX_ReturnsNegative()
        {
            IInterval<int> a = null;
            IInterval<int> b = new Interval<int>(0, 10);

            int result = IntervalComparer<int>.Default.Compare(a, b);

            Assert.That(result, Is.LessThan(0));
        }

        [Test]
        public void Compare_NullY_ReturnsPositive()
        {
            IInterval<int> a = new Interval<int>(0, 10);
            IInterval<int> b = null;

            int result = IntervalComparer<int>.Default.Compare(a, b);

            Assert.That(result, Is.GreaterThan(0));
        }

        [Test]
        public void Compare_NullYandY_ReturnsZero()
        {
            IInterval<int> a = null;
            IInterval<int> b = null;

            int result = IntervalComparer<int>.Default.Compare(a, b);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Compare_SameInstance_ReturnsZero()
        {
            IInterval<int> a = new Interval<int>(0, 10);
            IInterval<int> b = a;

            int result = IntervalComparer<int>.Default.Compare(a, b);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Compare_XEndGreaterThanYEnd_ReturnsPositive()
        {
            IInterval<int> a = new Interval<int>(0, 10);
            IInterval<int> b = new Interval<int>(0, 9);

            int result = IntervalComparer<int>.Default.Compare(a, b);

            Assert.That(result, Is.GreaterThan(0));
        }

        [Test]
        public void Compare_XEndLessThanYEnd_ReturnsNegative()
        {
            IInterval<int> a = new Interval<int>(0, 10);
            IInterval<int> b = new Interval<int>(0, 11);

            int result = IntervalComparer<int>.Default.Compare(a, b);

            Assert.That(result, Is.LessThan(0));
        }

        [Test]
        public void Compare_XStartGreaterThanYStart_ReturnsPositive()
        {
            IInterval<int> a = new Interval<int>(0, 10);
            IInterval<int> b = new Interval<int>(-1, 10);

            int result = IntervalComparer<int>.Default.Compare(a, b);

            Assert.That(result, Is.GreaterThan(0));
        }

        [Test]
        public void Compare_XStartLessThanYStart_ReturnsNegative()
        {
            IInterval<int> a = new Interval<int>(0, 10);
            IInterval<int> b = new Interval<int>(1, 10);

            int result = IntervalComparer<int>.Default.Compare(a, b);

            Assert.That(result, Is.LessThan(0));
        }
    }
}